using BankingSystemAPI.Repository;
using BankingSystemAPI.Models;
using Microsoft.AspNetCore.Mvc;
using BankingSystemAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankingSystemAPI.Controllers
{
    [Route("api/BankingSystem")]
    [ApiController]
    public class BankOperationsController : ControllerBase
    {
        private readonly IBankingOperationsRepository _bankingOperationsRepository;
        private readonly ILoggingService _logger;

        //List<UserAccount> users = new List<UserAccount>();

        public BankOperationsController(IBankingOperationsRepository bankingOperationsRepository,
            ILoggingService logger)
        {
            _bankingOperationsRepository = bankingOperationsRepository;
            _logger = logger;
        }

        /// <summary>
        /// Gets the list of all Users.
        /// </summary>
        /// <returns>The list of Users.</returns>
        [HttpGet]
        [Route("GetUserAccounts")]
        public IEnumerable<UserAccount> GetUserAccounts()
        {
            return _bankingOperationsRepository.GetUsers();
        }

        // POST api/<ValuesController>
        [HttpPost]
        [Route("AddUser")]
        public IActionResult PostNewUser([FromBody] UserAccountDto userDto)
        {
            if (userDto == null) {
                return BadRequest("User ca't be null");
            }

            if (userDto.Accounts == null)
            {
                _logger.LogInformation($"User must have at least one account associated.");
                return BadRequest("User must have at least one account associated.");
            }
            var accountUserId = Guid.NewGuid().ToString();
            var accountList = new List<Account>();
            if (userDto.Accounts != null)
            {
                foreach (var account in userDto.Accounts)
                {
                    if(account.Balance < 100)
                    {
                        return BadRequest($"The balance mustn't be less that 100$ at any time.");
                    }
                    accountList.Add(new Account
                    {
                        AccountUserId = accountUserId,
                        AccountNumber = Guid.NewGuid().ToString(),
                        Balance = account.Balance
                    });
                }
            }
            
            var user = new UserAccount()
            {
                userId = accountUserId,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Accounts = accountList
            };
            var result = _bankingOperationsRepository.AddUser(user);

            return Ok(result);

        }

        [HttpPost]
        [Route("CreateAccountForUser/{accountUserId}")]
        public IActionResult CreateAccount(string accountUserId, [FromBody] AccountDto accountDto)
        {
            if (accountDto == null)
            {
                return BadRequest("Account can't be null");
            }
            var userAccount = _bankingOperationsRepository.GetUser(accountUserId);

            if (userAccount == null)
            {
                return NotFound($"User Account couldn't be found");
            }

            if (accountDto.Balance < 100)
            {
                _logger.LogInformation($"Account must have minimum of 100$ at any time");
                return BadRequest($"Account must have minimum of 100$ at any time");
            }
            //var accountUserId = Guid.NewGuid().ToString();
            var account = new Account
            {
                AccountUserId = userAccount.userId,
                AccountNumber = Guid.NewGuid().ToString(),
                Balance = accountDto.Balance
            };

            var newAccount = _bankingOperationsRepository.CreateAccountForUser(userAccount, account);

            return Ok(newAccount);

        }

        // DELETE api/<ValuesController>/5
        [HttpDelete]
        [Route("DeleteAccountForUser/{accountUserId}")]
        public IActionResult DeleteAccountFromUser(string accountUserId, string accountNumber)
        {
            var userAccount = _bankingOperationsRepository.GetUser(accountUserId);

            if(userAccount == null)
            {
                _logger.LogInformation($"User account not found -{accountUserId}");
                return NotFound($"User account not found - {accountUserId}");
            }

            var account = userAccount.Accounts.Where(x => x.AccountNumber == accountNumber).FirstOrDefault();
            if (account == null) {
                _logger.LogInformation($"The account number was not found for the user - {accountNumber}");
                return NotFound($"The account number was not found for the user - {accountNumber}");
            }

            _bankingOperationsRepository.DeleteAccountForUser(userAccount, accountNumber);
            _logger.LogInformation($"Deleted account from user {accountUserId}");
            return Ok();
        }
    }
}
