using BankingSystemAPI.Models;
using BankingSystemAPI.Repository;
using BankingSystemAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BankingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IBankOperationsService _bankOperationsService;
        private readonly ITransactionService _transactionService;
        private readonly ILoggingService _logger;

        public TransactionController(IBankOperationsService bankOperationsService, ITransactionService transactionService, ILoggingService logger)
        {
            _bankOperationsService = bankOperationsService;
            _transactionService = transactionService;
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
        public async Task<IActionResult> DepositAsync(string accountUserId, string accountNumber, decimal amount)
        {
            var userAccount = await _bankOperationsService.GetUserAccountAsync(accountUserId);
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
            var errors = await _transactionService.ValidateDepositAmountAsync(amount);
            if (errors.Any())
            {
                return BadRequest(errors);
            }

            var result = await _transactionService.DepositAmountAsync(userAccount, account, amount);
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
        public async Task<IActionResult> WithdrawAsync(string accountUserId, string accountNumber, decimal amount)
        {
            var userAccount = await _bankOperationsService.GetUserAccountAsync(accountUserId);
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
            var validationErrors = await _transactionService.ValidateWithdrawAmountAsync(account, amount);
            if(validationErrors.Any())
            {
                return BadRequest(validationErrors);
            }

            var result = new Account();
            
            result = await _transactionService.WithdrawAmountAsync(userAccount, account, amount);
            
            return Ok(result);

        }
    }
}
