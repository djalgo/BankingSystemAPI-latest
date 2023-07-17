using BankingSystemAPI.Models;

namespace BankingSystemAPI.Repository
{
    public interface IBankingOperationsRepository
    {
        UserAccount AddUser(UserAccount user);
        IEnumerable<UserAccount> GetUsers();
        UserAccount GetUser(string id);
        UserAccount CreateAccountForUser(UserAccount user, Account account);
        void DeleteAccountForUser(UserAccount user, string accountNumber);

        Account DepositAmount(UserAccount user, Account account, decimal amount);

        Account WithdrawAmount(UserAccount user, Account account, decimal amount);
    }
}
