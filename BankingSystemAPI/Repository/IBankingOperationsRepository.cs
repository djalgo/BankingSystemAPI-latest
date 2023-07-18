using BankingSystemAPI.Models;

namespace BankingSystemAPI.Repository
{
    public interface IBankingOperationsRepository
    {
        /// <summary>
        /// Add new user account
        /// </summary>
        /// <param name="user"></param>
        /// <returns>New created user account</returns>
        UserAccount AddUser(UserAccount user);

        /// <summary>
        /// Get all the user accounts
        /// </summary>
        /// <returns>Returns the list of user account</returns>
        IEnumerable<UserAccount> GetUsers();

        /// <summary>
        /// Get a particular user account by user ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A user account</returns>
        UserAccount GetUser(string id);

        /// <summary>
        /// Creates a new account for user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="account"></param>
        /// <returns>Returns new created account</returns>
        UserAccount CreateAccountForUser(UserAccount user, Account account);

        /// <summary>
        /// Deletes an existing account
        /// </summary>
        /// <param name="user"></param>
        /// <param name="accountNumber"></param>
        void DeleteAccountForUser(UserAccount user, string accountNumber);

        /// <summary>
        /// Deposits an amount to the account
        /// </summary>
        /// <param name="user"></param>
        /// <param name="account"></param>
        /// <param name="amount"></param>
        /// <returns>Returns the account with updated balance</returns>
        Account DepositAmount(UserAccount user, Account account, decimal amount);

        /// <summary>
        /// Withdraws an amount from the account
        /// </summary>
        /// <param name="user"></param>
        /// <param name="account"></param>
        /// <param name="amount"></param>
        ///  /// <returns>Returns the account with updated balance</returns>
        Account WithdrawAmount(UserAccount user, Account account, decimal amount);
    }
}
