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
    internal class BankOperationsRepositoryTestsGetUsers
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
            var accountDto =
                new AccountDto
                {
                    Balance = 120,

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

            _service.GetUserAccountAsync(Arg.Any<string>()).Returns(userAccount);
            _service.CreateAccountForUserAsync(Arg.Any<UserAccount>(), Arg.Any<AccountDto>()).Returns(responseAccount);
            //Act
            var result = _controller.CreateAccountForUserAsync(id, accountDto).Result;


            //Assert
            var okResult = result as ObjectResult;

            Assert.Multiple(() =>
            {
                Assert.That(200, Is.EqualTo(okResult.StatusCode));
                Assert.That(okResult.Value, Is.EqualTo(responseAccount));
            });

        }

        [Test]
        public void CreateAccount_ReturnsNotFound_WhenUserNotFound()
        {

            //Arrange
            var id = Guid.NewGuid().ToString();
            var account = Guid.NewGuid().ToString();
            UserAccount? userAccount = null;
            var accountDto =
                new AccountDto
                {
                    Balance = 120,

                };



            _service.GetUserAccountAsync(Arg.Any<string>()).Returns(userAccount);
            
            //Act
            var result = _controller.CreateAccountForUserAsync(id, accountDto).Result;


            //Assert
            var notFoundResult = result as ObjectResult;

            Assert.Multiple(() =>
            {
                Assert.That(404, Is.EqualTo(notFoundResult.StatusCode));
                Assert.That(notFoundResult.Value, Is.EqualTo($"User Account couldn't be found"));
            });

        }

        [Test]
        public void CreateAccount_ReturnsBadRequest_WhenBalanceLessThan100()
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
            var accountDto =
                new AccountDto
                {
                    Balance = 80,

                };


            _service.GetUserAccountAsync(Arg.Any<string>()).Returns(userAccount);
           
            //Act
            var result = _controller.CreateAccountForUserAsync(id, accountDto).Result;


            //Assert
            var okResult = result as ObjectResult;

            Assert.Multiple(() =>
            {
                Assert.That(400, Is.EqualTo(okResult.StatusCode));
                Assert.That(okResult.Value, Is.EqualTo($"Account must have minimum of 100$ at any time"));
            });

        }


    }
}
