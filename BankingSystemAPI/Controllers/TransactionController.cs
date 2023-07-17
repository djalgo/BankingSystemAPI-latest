using BankingSystemAPI.Models;
using BankingSystemAPI.Repository;
using BankingSystemAPI.Services;
using Microsoft.AspNetCore.Mvc;


namespace BankingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IBankingOperationsRepository _bankingOperationsRepository;
        private readonly ILoggingService _logger;

        public TransactionController(IBankingOperationsRepository bankingOperationsRepository, ILoggingService logger)
        {
            _bankingOperationsRepository = bankingOperationsRepository;
            _logger = logger;
        }
        // GET: api/<TransactionController>
        [HttpPut]
        [Route("deposit/{accountUserId}/{accountNumber}/{amount}")]
        public IActionResult Deposit(string accountUserId, string accountNumber, decimal amount)
        {
            var userAccount = _bankingOperationsRepository.GetUser(accountUserId);
            if (userAccount == null)
            {
                _logger.LogInformation($"User not found - {accountUserId}");
                return NotFound($"User not found - {accountUserId}");
            }

            var account = userAccount.Accounts.Where(x => x.AccountNumber == accountNumber).FirstOrDefault();
            if (account == null)
            {
                _logger.LogInformation($"Account not found - {accountNumber}");
                return NotFound($"Account not found - {accountNumber}");
            }

            if(amount > 10000 || amount <= 0)
            {
                _logger.LogInformation($"The amount must be non-negative or non-zero or less than 10000$ in a single transaction.");
                return BadRequest($"The amount must be non-negative or non-zero or less than 10000$ in a single transaction.");
            }

            var result = _bankingOperationsRepository.DepositAmount(userAccount, account, amount);
            return Ok(result);

        }

        [HttpPut]
        [Route("withdraw/{accountUserId}/{accountNumber}/{amount}")]
        public IActionResult Withdraw(string accountUserId, string accountNumber, decimal amount)
        {
            var userAccount = _bankingOperationsRepository.GetUser(accountUserId);
            if (userAccount == null)
            {
                return NotFound($"User not found.");
            }

            var account = userAccount.Accounts.Where(x => x.AccountNumber == accountNumber).FirstOrDefault();
            if (account == null)
            {
                return NotFound($"The account number was not found for the user.");
            }

            var balance = account.Balance;
            
            if(amount > balance)
            {
                _logger.LogInformation($"Insufficient funds - {accountNumber}");
                return BadRequest($"Insufficient funds - {accountNumber}");
            }
            var balanceAfterWithdrawal = balance - amount;
            var percentWithdrawal = (int) amount / balance * 100;

            if(percentWithdrawal > 90) {
                return BadRequest($"Invalid Amount. Withdrawing more than 90% of the current balance is not allowed. " +
                    $"Current withdrawal amount is {percentWithdrawal}% of the balance.");
            }

            if( balanceAfterWithdrawal < 100)
            {
                return BadRequest($"Invalid Amount. The account must have at least 100$ at any point. " +
                    $"Current amount will be {balanceAfterWithdrawal}$.");
            }


            var result = _bankingOperationsRepository.WithdrawAmount(userAccount, account, amount);
            return Ok(result);

        }
    }
}
