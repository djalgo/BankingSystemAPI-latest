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

namespace BankingSystemAPI.Tests.ControllersTest.BankOperationsControllerTests
{
    [TestFixture]
    internal class BankOperationsControllerTestsDeleteAccountForUser
    {
        private BankOperationsController _controller;
        private IBankOperationsService _service;
        private ILoggingService _logger;

        [SetUp]
        public void SetUp()
        {
            //using NSubstitute for mocking here because I have experience with this framework
            _service = Substitute.For<IBankOperationsService>();
            _logger = Substitute.For<ILoggingService>();
            _controller = new BankOperationsController(_service, _logger);
        }

        [Test]
        public void DeleteAccount_ReturnsOkResponse_WhenDeleted()
        {

            //Arrange
            var id = Guid.NewGuid().ToString();
            var account1 = Guid.NewGuid().ToString();
            var account2 = Guid.NewGuid().ToString();
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
                        AccountNumber = account1
                    },
                     new Account
                    {
                        AccountUserId = id,
                        Balance = 200,
                        AccountNumber = account2
                    }
                }
            };

            _service.GetUserAccountAsync(Arg.Any<string>()).Returns(userAccount);
            _service.DeleteAccountForUserAsync(Arg.Any<UserAccount>(), Arg.Any<string>());

            //Act

            _controller.DeleteAccountFromUserAsync(id, account2);

            //Assert
            
           _logger.Received(1).LogInformation($"Deleted account from user {id}");

        }

        [Test]
        public void DeleteAccount_ReturnsNotFound_WhenAccountDoesntExist()
        {

            //Arrange
            var id = Guid.NewGuid().ToString();
            var account1 = Guid.NewGuid().ToString();
            var account2 = Guid.NewGuid().ToString();
            var account3 = "test-account-not-existing";
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
                        AccountNumber = account1
                    },
                     new Account
                    {
                        AccountUserId = id,
                        Balance = 200,
                        AccountNumber = account2
                    }
                }
            };

            _service.GetUserAccountAsync(Arg.Any<string>()).Returns(userAccount);

            //Act

            var result = _controller.DeleteAccountFromUserAsync(id, account3).Result;


            //Assert

            var NotFoundResult = result as ObjectResult;
            Assert.That(404, Is.EqualTo(NotFoundResult.StatusCode));

            _logger.Received(1).LogError($"The account number was not found for the user - {account3}");
        }


        [Test]
        public void DeleteAccount_ReturnsNotFound_WhenUserAccountDoesntExist()
        {

            //Arrange
            var id = Guid.NewGuid().ToString();
            var account1 = Guid.NewGuid().ToString();
            var account2 = Guid.NewGuid().ToString();
            var account3 = "test-account-not-existing";
            UserAccount userAccount = null;

            _service.GetUserAccountAsync(Arg.Any<string>()).Returns(userAccount);

            //Act

            var result = _controller.DeleteAccountFromUserAsync(id, account3).Result;


            //Assert

            var NotFoundResult = result as ObjectResult;
            Assert.That(404, Is.EqualTo(NotFoundResult.StatusCode));

            _logger.Received(1).LogError($"User account not found -{id}");



        }
    }
}
