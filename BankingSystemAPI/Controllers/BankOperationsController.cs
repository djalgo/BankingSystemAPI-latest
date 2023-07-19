using BankingSystemAPI.Repository;
using BankingSystemAPI.Models;
using Microsoft.AspNetCore.Mvc;
using BankingSystemAPI.Services;
using NuGet.Protocol;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankingSystemAPI.Controllers
{
    [Route("api/BankingSystem")]
    [ApiController]
    public class BankOperationsController : ControllerBase
    {
        private readonly IBankOperationsService _bankOperationsService;
        private readonly ILoggingService _logger;

        public BankOperationsController(IBankOperationsService bankOperationsService,
            ILoggingService logger)
        {
            _bankOperationsService = bankOperationsService ?? throw new ArgumentNullException(nameof(bankOperationsService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets the list of all Users.
        /// </summary>
        /// <returns>The list of Users.</returns>
        [HttpGet]
        [Route("GetUserAccounts")]
        public async Task<IActionResult> GetUserAccountsAsync()
        {
            var result = await _bankOperationsService.GetUserAccountsAsync();
            return Ok(result);
        }

        /// <summary>
        /// Post new user account.
        /// </summary>
        /// <returns>The new user created.</returns>
        // POST api/<ValuesController>
        [HttpPost]
        [Route("AddNewUserAccount")]
        public async Task<IActionResult> AddNewUserAccountAsync([FromBody] UserAccountDto userDto)
        {
            if (userDto == null) {
                _logger.LogError($"User can't be null");
                return BadRequest($"User can't be null");
            }

            if (userDto.Accounts == null)
            {
                _logger.LogError($"User must have at least one account associated.");
                return BadRequest("User must have at least one account associated.");
            }
            
            if (await EmailAlreadyExistsAsync(userDto.Email)){
                return BadRequest("Email should be unique for a record.");
            }

            var result = new UserAccount();
            
            result = await _bankOperationsService.AddNewUserAccountAsync(userDto);
            
            return Ok(result);

        }

        /// <summary>
        /// Creates a new account for user.
        /// </summary>
        /// <returns>The new created acount</returns>
        [HttpPost]
        [Route("CreateAccountForUser/{accountUserId}")]
        public async Task<IActionResult> CreateAccountForUserAsync(string accountUserId, [FromBody] AccountDto accountDto)
        {
            if (accountDto == null)
            {
                _logger.LogError($"Account can't be null");
                return BadRequest("Account can't be null");
            }
            var userAccount = await _bankOperationsService.GetUserAccountAsync(accountUserId);

            if (userAccount == null)
            {
                _logger.LogError($"User Account couldn't be found");
                return NotFound($"User Account couldn't be found");
            }

            if (accountDto.Balance < 100)
            {
                _logger.LogError($"Account must have minimum of 100$ at any time");
                return BadRequest($"Account must have minimum of 100$ at any time");
            }
            
            var newAccount = await _bankOperationsService.CreateAccountForUserAsync(userAccount, accountDto);

            return Ok(newAccount);

        }

        /// <summary>
        /// Deletes a acount for the user.
        /// </summary>
        /// <returns></returns>
        // DELETE api/<ValuesController>/5
        [HttpDelete]
        [Route("DeleteAccountForUser/{accountUserId}/{accountNumber}")]
        public async Task<IActionResult> DeleteAccountFromUserAsync(string accountUserId, string accountNumber)
        {
            var userAccount = await _bankOperationsService.GetUserAccountAsync(accountUserId);

            if(userAccount == null)
            {
                _logger.LogError($"User account not found -{accountUserId}");
                return NotFound($"User account not found - {accountUserId}");
            }

            var account = userAccount.Accounts.Where(x => x.AccountNumber == accountNumber).FirstOrDefault();
            if (account == null) {
                _logger.LogError($"The account number was not found for the user - {accountNumber}");
                return NotFound($"The account number was not found for the user - {accountNumber}");
            }

            await _bankOperationsService.DeleteAccountForUserAsync(userAccount, accountNumber);
            _logger.LogInformation($"Deleted account from user {accountUserId}");
            return Ok();
        }

        /// <summary>
        /// Validates for existing user
        /// </summary>
        /// <param name="email"></param>
        /// <returns>True or false</returns>
        private async Task<bool> EmailAlreadyExistsAsync(string email)
        {
            if(email != null)
            {
                var users = await _bankOperationsService.GetUserAccountsAsync();
                if(users == null)
                {
                    return false;
                }
                else
                {
                    return users.Any(x => x.Email == email);    
                }
            }
            return false;
        }
    }
}
