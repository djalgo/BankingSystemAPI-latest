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

        /// <summary>
        /// Deposits the requested amount
        /// </summary>
        /// <param name="accountUserId"></param>
        /// <param name="accountNumber"></param>
        /// <param name="amount"></param>
        /// <returns>The account detail with updated amount</returns>
        // GET: api/<TransactionController>
        [HttpPut]
        [Route("deposit/{accountUserId}/{accountNumber}/{amount}")]
        public IActionResult Deposit(string accountUserId, string accountNumber, decimal amount)
        {
            var userAccount = _bankingOperationsRepository.GetUser(accountUserId);
            if (userAccount == null)
            {
                _logger.LogError($"User not found - {accountUserId}");
                return NotFound($"User not found - {accountUserId}");
            }

            var account = userAccount.Accounts.Where(x => x.AccountNumber == accountNumber).FirstOrDefault();
            if (account == null)
            {
                _logger.LogError($"Account not found - {accountNumber}");
                return NotFound($"Account not found - {accountNumber}");
            }

            if(amount > 10000 || amount <= 0)
            {
                _logger.LogError($"The amount must be non-negative or non-zero or less than 10000$ in a single transaction.");
                return BadRequest($"The amount must be non-negative or non-zero or less than 10000$ in a single transaction.");
            }

            var result = _bankingOperationsRepository.DepositAmount(userAccount, account, amount);
            return Ok(result);

        }

        /// <summary>
        /// Withdraw the requested amount
        /// </summary>
        /// <param name="accountUserId"></param>
        /// <param name="accountNumber"></param>
        /// <param name="amount"></param>
        /// <returns>The account detail with updated amount.</returns>
        [HttpPut]
        [Route("withdraw/{accountUserId}/{accountNumber}/{amount}")]
        public IActionResult Withdraw(string accountUserId, string accountNumber, decimal amount)
        {
            var userAccount = _bankingOperationsRepository.GetUser(accountUserId);
            if (userAccount == null)
            {
                _logger.LogError($"User not found - {accountUserId}");
                return NotFound($"User not found - {accountUserId}");
            }

            var account = userAccount.Accounts.Where(x => x.AccountNumber == accountNumber).FirstOrDefault();
            if (account == null)
            {
                _logger.LogError($"The account number was not found for the user - {accountNumber}");
                return NotFound($"The account number was not found for the user - {accountNumber}");
            }

            var balance = account.Balance;
            
            if(amount > balance)
            {
                _logger.LogError($"Insufficient funds - {accountNumber}");
                return BadRequest($"Insufficient funds - {accountNumber}");
            }
            var balanceAfterWithdrawal = balance - amount;
            var percentWithdrawal = (int) amount / balance * 100;

            if(percentWithdrawal > 90) {
                _logger.LogError($"Invalid Amount. Withdrawing more than 90% of the current balance is not allowed. " +
                    $"Current withdrawal amount is {percentWithdrawal}% of the balance.");
                return BadRequest($"Invalid Amount. Withdrawing more than 90% of the current balance is not allowed. " +
                    $"Current withdrawal amount is {percentWithdrawal}% of the balance.");
            }

            if( balanceAfterWithdrawal < 100)
            {
                _logger.LogError($"Invalid Amount. The account must have at least 100$ at any point. " +
                    $"Current amount will be {balanceAfterWithdrawal}$.");
                return BadRequest($"Invalid Amount. The account must have at least 100$ at any point. " +
                    $"Current amount will be {balanceAfterWithdrawal}$.");
            }


            var result = _bankingOperationsRepository.WithdrawAmount(userAccount, account, amount);
            return Ok(result);

        }
    }
}
