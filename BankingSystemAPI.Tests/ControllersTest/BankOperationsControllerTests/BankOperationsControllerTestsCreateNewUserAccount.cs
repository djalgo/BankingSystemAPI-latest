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
    internal class BankOperationsControllerTestsCreateNewUserAccount
    {
        private BankOperationsController _controller;
        private IBankingOperationsRepository _repository;
        private ILoggingService _logger;

        [SetUp]
        public void SetUp()
        {
            //using NSubstitute for mocking here because I have experience with this framework
            _repository = Substitute.For<IBankingOperationsRepository>();
            _logger = Substitute.For<ILoggingService>();
            _controller = new BankOperationsController(_repository, _logger);
        }

        [Test]
        public void CreateNewUserAccount_ReturnsUserAccounts_WhenCreated()
        {

            //Arrange
            var id = Guid.NewGuid().ToString();
            var account = Guid.NewGuid().ToString();
            var userAccount = new UserAccountDto
            {
                FirstName = "test f name",
                LastName = "test l name",
                Email = "test-email",
                Accounts = new List<AccountDto> { 
                    new AccountDto
                    {
                        Balance = 100,
                    }
                }
            };
            var userAccountResponse =
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
                            Balance = 100,
                            CreatedDate = DateTime.Now
                        }
                    }
                };

            _repository.AddUser(Arg.Any<UserAccount>()).Returns(userAccountResponse);
            //Act
            var result = _controller.PostNewUser(userAccount);


            //Assert
            var okResult = result as ObjectResult;

            Assert.Multiple(() =>
            {
                Assert.That(200, Is.EqualTo(okResult.StatusCode));
                Assert.That(okResult.Value, Is.EqualTo(userAccountResponse));
            });

        }

        [Test]
        public void CreateNewUserAccount_ReturnsBadRequest_WhenAccountNotIncluded()
        {

            //Arrange
            var id = Guid.NewGuid().ToString();
            var account = Guid.NewGuid().ToString();
            var userAccount = new UserAccountDto
            {
                FirstName = "test f name",
                LastName = "test l name",
                Email = "test-email"
                
            };
            

            //_repository.AddUser(Arg.Any<UserAccount>()).Returns(userAccountResponse);
            //Act
            var result = _controller.PostNewUser(userAccount);


            //Assert
            var badObjectResult = result as ObjectResult;
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(badObjectResult);
                Assert.That(400, Is.EqualTo(badObjectResult.StatusCode));
            });
            


        }

        [Test]
        public void CreateNewUserAccount_ReturnsBadRequest_WhenAccountBalanceisLessThan100()
        {

            //Arrange
            var id = Guid.NewGuid().ToString();
            var account = Guid.NewGuid().ToString();
            var userAccount = new UserAccountDto
            {
                FirstName = "test f name",
                LastName = "test l name",
                Email = "test-email"

            };


            //_repository.AddUser(Arg.Any<UserAccount>()).Returns(userAccountResponse);
            //Act
            var result = _controller.PostNewUser(userAccount);


            //Assert
            var badObjectResult = result as ObjectResult;
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(badObjectResult);
                Assert.That(400, Is.EqualTo(badObjectResult.StatusCode));
            });



        }
    }
}
