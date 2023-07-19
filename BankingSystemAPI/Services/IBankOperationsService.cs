using BankingSystemAPI.Models;

namespace BankingSystemAPI.Services
{
    public interface IBankOperationsService
    {
        /// <summary>
        /// Get all user accounts
        /// </summary>
        /// <returns>List of user accounts</returns>
        Task<IEnumerable<UserAccount>> GetUserAccountsAsync();

        /// <summary>
        /// Adds a new user account
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns>New user account</returns>
        Task<UserAccount> AddNewUserAccountAsync(UserAccountDto userAccount);

        /// <summary>
        /// Get user account
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>user account</returns>
        Task<UserAccount> GetUserAccountAsync(string userId);

        /// <summary>
        /// Creates a new account for user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="account"></param>
        /// <returns>User account with new account</returns>
        Task<UserAccount> CreateAccountForUserAsync(UserAccount user, AccountDto account);

        /// <summary>
        /// Delete Account for user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        Task DeleteAccountForUserAsync(UserAccount user, string accountNumber);
    }
}
