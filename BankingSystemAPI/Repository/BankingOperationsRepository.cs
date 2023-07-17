using BankingSystemAPI.Models;
using BankingSystemAPI.Services;

namespace BankingSystemAPI.Repository
{
    public class BankingOperationsRepository : IBankingOperationsRepository
    {
        private readonly ILoggingService _logger;
        List<UserAccount> users = new List<UserAccount>();
        public BankingOperationsRepository(ILoggingService logger)
        {
            _logger = logger;
        }

        public UserAccount AddUser(UserAccount user)
        {
            users.Add(user);
            return user;
        }

        public UserAccount CreateAccountForUser(UserAccount user, Account account)
        {
            user.Accounts.Add(account);
            return user;
        }

        public void DeleteAccountForUser(UserAccount user, string accountNumber)
        {
            var account = user.Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
            user.Accounts.Remove(account);
        }

        public Account DepositAmount(UserAccount user, Account account, decimal amount)
        {
            var balance = account.Balance;
            balance += amount;
            account.Balance = balance;
            return account;
        }

        public UserAccount GetUser(string id)
        {
            var userAccount = users
                .Where(x => x.userId == id)
                .FirstOrDefault();
            return userAccount;
        }

        public IEnumerable<UserAccount> GetUsers()
        {
            return users;
        }

        public Account WithdrawAmount(UserAccount user, Account account, decimal amount)
        {
            var balance = account.Balance;
            balance -= amount;
            account.Balance = balance;
            return account;
        }
    }
}
