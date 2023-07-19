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

        public async Task<UserAccount> AddUserAsync(UserAccount user)
        {
            await Task.Run(() => users.Add(user));
            return user;
        }

        public async Task<UserAccount> CreateAccountForUserAsync(UserAccount user, Account account)
        {
            await Task.Run(() => user.Accounts.Add(account));
            return user;
        }

        public async Task DeleteAccountForUserAsync(UserAccount user, string accountNumber)
        {
            var account = user.Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
            await Task.Run(() => user.Accounts.Remove(account));
        }

        public async Task<Account> DepositAmountAsync(UserAccount user, Account account, decimal amount)
        {
            var balance = account.Balance;
            balance += amount;
            await Task.Run(() => account.Balance = balance);
            return account;
        }

        public async Task<UserAccount> GetUserAsync(string id)
        {
            var userAccount = await Task.Run(() => users
                .Where(x => x.userId == id)
                .FirstOrDefault());
            return userAccount;
        }

        public async Task<IEnumerable<UserAccount>> GetUsersAsync()
        {
            return await Task.Run(() => users);
        }

        public async Task<Account> WithdrawAmountAsync(UserAccount user, Account account, decimal amount)
        {
            var balance = account.Balance;
            balance -= amount;
            await Task.Run(() => account.Balance = balance);
            return account;
        }
    }
}
