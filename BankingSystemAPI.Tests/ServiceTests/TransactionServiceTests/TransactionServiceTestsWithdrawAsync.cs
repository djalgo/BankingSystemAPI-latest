using BankingSystemAPI.Models;
using BankingSystemAPI.Repository;
using BankingSystemAPI.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystemAPI.Tests.ServiceTests.TransactionServiceTests
{
    [TestFixture]
    internal class TransactionServiceTestsWithdrawAsync
    {
        private TransactionService _service;
        private IBankingOperationsRepository _repository;
        private ILoggingService _logger;

        [SetUp]
        public void SetUp()
        {
            //using NSubstitute for mocking here because I have experience with this framework
            _repository = Substitute.For<IBankingOperationsRepository>();
            _logger = Substitute.For<ILoggingService>();
            _service = new TransactionService(_repository, _logger);
        }


        [Test]
        public void WithdrawAmountAsync_WhenSuccessful()
        {
            var id = Guid.NewGuid().ToString();
            decimal amount = 200;
            var accountNumber = Guid.NewGuid().ToString();
            var userAccount = new UserAccount
            {
                userId = id,
                FirstName = "test f name",
                LastName = "test l name",
                Email = "test-email",
                Accounts = new List<Account> {
                    new Account
                    {
                        AccountUserId = id,
                        Balance = 300,
                        AccountNumber = accountNumber
                    }
                }
            };
            var account = new Account
            {
                Balance = 100
            };

            var responseAccount = new Account
            {

                AccountUserId = id,
                Balance = 200,
                AccountNumber = accountNumber

            };

            _repository.WithdrawAmountAsync(Arg.Any<UserAccount>(), Arg.Any<Account>(), Arg.Any<decimal>()).Returns(responseAccount);

            //Act
            var result = _service.WithdrawAmountAsync(userAccount, account, amount).Result;

            //Assert
            Assert.That(result, Is.EqualTo(responseAccount));
        }

    }
}
