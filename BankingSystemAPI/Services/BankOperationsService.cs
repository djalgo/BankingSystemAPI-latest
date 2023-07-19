using BankingSystemAPI.Models;
using BankingSystemAPI.Repository;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Net.Http;

namespace BankingSystemAPI.Services
{
    /// <summary>
    /// Service to handle banking operations
    /// </summary>
    public class BankOperationsService : IBankOperationsService
    {
        private readonly IBankingOperationsRepository _bankingOperationsRepository;
        private readonly ILoggingService _logger;

        public BankOperationsService(IBankingOperationsRepository bankingOperationsRepository, ILoggingService logger)
        {
            _bankingOperationsRepository = bankingOperationsRepository ?? throw new ArgumentNullException(nameof(bankingOperationsRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        
        public async Task<IEnumerable<UserAccount>> GetUserAccountsAsync()
        {
            var result = await _bankingOperationsRepository.GetUsersAsync();
            return result;
        }

        public async Task<UserAccount> AddNewUserAccountAsync(UserAccountDto userAccount) 
        {
            var accountUserId = Guid.NewGuid().ToString();
            var accountList = new List<Account>();

            foreach (var account in userAccount.Accounts)
            {
                accountList.Add(new Account
                {
                    AccountUserId = accountUserId,
                    AccountNumber = Guid.NewGuid().ToString(),
                    Balance = account.Balance
                });
            }

            var user = new UserAccount()
            {
                userId = accountUserId,
                FirstName = userAccount.FirstName,
                LastName = userAccount.LastName,
                Email = userAccount.Email,
                Accounts = accountList
            };

            return await _bankingOperationsRepository.AddUserAsync(user);
        }

        public async Task<UserAccount> GetUserAccountAsync(string userId)
        {
            return await _bankingOperationsRepository.GetUserAsync(userId);
        }

        public async Task<UserAccount> CreateAccountForUserAsync(UserAccount user, AccountDto userAccount)
        {
            var account = new Account
            {
                AccountUserId = user.userId,
                AccountNumber = Guid.NewGuid().ToString(),
                Balance = userAccount.Balance
            };

            return await _bankingOperationsRepository.CreateAccountForUserAsync(user, account);
        }

        public async Task DeleteAccountForUserAsync(UserAccount user, string accountNumber)
        {
            await _bankingOperationsRepository.DeleteAccountForUserAsync(user, accountNumber);
            _logger.LogInformation($"Account deleted - {accountNumber}");
        }
    }
}
