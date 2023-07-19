using BankingSystemAPI.Models;

namespace BankingSystemAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITransactionService
    {
        /// <summary>
        /// Deposit amount
        /// </summary>
        /// <param name="user"></param>
        /// <param name="account"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        Task<Account> DepositAmountAsync(UserAccount user, Account account, decimal amount);

        /// <summary>
        /// Withdraws amount
        /// </summary>
        /// <param name="user"></param>
        /// <param name="account"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        Task<Account> WithdrawAmountAsync(UserAccount user, Account account, decimal amount);

        /// <summary>
        /// Validate withdrawn amount
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<AmountValidation>> ValidateWithdrawAmountAsync(Account account, decimal amount);

        /// <summary>
        /// validate deposit amount
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<AmountValidation>> ValidateDepositAmountAsync(decimal amount);
    }
}
