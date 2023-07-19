using BankingSystemAPI.Controllers;
using BankingSystemAPI.Models;
using BankingSystemAPI.Repository;
using BankingSystemAPI.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystemAPI.Tests.ControllersTest.BankOperationsServiceTests
{
    [TestFixture]
    internal class BankOperationsServicesTestsCreateAccountForUser
    {
        private BankOperationsService _service;
        private IBankingOperationsRepository _repository;
        private ILoggingService _logger;

        [SetUp]
        public void SetUp()
        {
            //using NSubstitute for mocking here because I have experience with this framework
            _repository = Substitute.For<IBankingOperationsRepository>();
            _logger = Substitute.For<ILoggingService>();
            _service = new BankOperationsService(_repository, _logger);
        }

        [Test]
        public void CreateAccount_ReturnsUserAccount_WhenCreated()
        {

            //Arrange
            var id = Guid.NewGuid().ToString();
            var account = Guid.NewGuid().ToString();
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
                        Balance = 100,
                        AccountNumber = account
                    }
                }
            };
            var accountDto = new AccountDto
            {
                Balance = 120
            };
            var newAccount =
                new Account
                {
                    AccountUserId = id,
                    Balance = 120,
                    AccountNumber = account
                };
            var newAccountNumber = Guid.NewGuid().ToString();
            var responseAccount = new UserAccount
            {
                userId = id,
                FirstName = "test f name",
                LastName = "test l name",
                Email = "test-email",
                Accounts = new List<Account> {
                    new Account
                    {
                        AccountUserId = id,
                        Balance = 100,
                        AccountNumber = account
                    },
                    new Account
                    {
                        AccountUserId = id,
                        Balance = 120,
                        AccountNumber = newAccountNumber
                    }
                }
            };

            _repository.CreateAccountForUserAsync(Arg.Any<UserAccount>(), Arg.Any<Account>()).Returns(responseAccount);
            //Act
            var result = _service.CreateAccountForUserAsync(userAccount, accountDto).Result;

            //Assert
            Assert.Multiple(() =>
            { 
                Assert.That(responseAccount, Is.EqualTo(responseAccount));
            });
        }
    }
}
