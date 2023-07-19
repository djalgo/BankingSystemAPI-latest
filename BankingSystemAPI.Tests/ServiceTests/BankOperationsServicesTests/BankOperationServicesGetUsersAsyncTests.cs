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

namespace BankingSystemAPI.Tests.ServiceTests.BankOperationsServicesTests
{
    [TestFixture]
    internal class BankOperationServicesGetUsersAsyncTests
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
        public void GetUserAccountsAsync_WhenSuccessful()
        {
            //Arrange
            var id = Guid.NewGuid().ToString();
            var account = Guid.NewGuid().ToString();
            var userAccounts = new List<UserAccount> {
                new UserAccount
                {
                    userId = id,
                    FirstName = "first-name",
                    LastName = "last-name",
                    Email = "test@gmail.com",
                    CreatedDate = DateTime.Now,
                    Accounts = new List<Account>
                    {
                        new Account
                        {
                            AccountNumber = account,
                            Balance = 200,
                            CreatedDate = DateTime.Now
                        }
                    }
                }

            };

            _repository.GetUsersAsync().Returns(userAccounts);
            //Act
            var result = _service.GetUserAccountsAsync().Result;
            
            //Assert
            Assert.That(result, Is.EqualTo(userAccounts));

        }

        [Test]
        public void GetUserAccountsAsync_WhenNoneExists_ReturnsEmpty()
        {
            //Arrange
            var id = Guid.NewGuid().ToString();
            var account = Guid.NewGuid().ToString();
            List<UserAccount> userAccounts = new List<UserAccount>();

            _repository.GetUsersAsync().Returns(userAccounts);
            //Act
            var result = _service.GetUserAccountsAsync().Result;

            //Assert
            Assert.That(result, Is.EqualTo(userAccounts));

        }
    }
}
     
