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
    internal class TransactionControllerTestsWithdrawAmount
    {
        private TransactionController _controller;
        private IBankingOperationsRepository _repository;
        private ILoggingService _logger;

        [SetUp]
        public void SetUp()
        {
            //using NSubstitute for mocking here because I have experience with this framework
            _repository = Substitute.For<IBankingOperationsRepository>();
            _logger = Substitute.For<ILoggingService>();
            _controller = new TransactionController(_repository, _logger);
        }

        [Test]
        public void WithdrawAmount_WhenSuccessful()
        {

            //Arrange
            var id = Guid.NewGuid().ToString();
            decimal amount = 1000;
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
                        Balance = 3000,
                        AccountNumber = account
                    }
                }
            };

            var newAccountNumber = Guid.NewGuid().ToString();
            var responseAccount = new Account
            {

                AccountUserId = id,
                Balance = 2000,
                AccountNumber = account

            };

            _repository.GetUser(Arg.Any<string>()).Returns(userAccount);
            _repository.DepositAmount(Arg.Any<UserAccount>(), Arg.Any<Account>(), Arg.Any<decimal>()).Returns(responseAccount);
            //Act
            var result = _controller.Deposit(id, account, amount);


            //Assert
            var okResult = result as ObjectResult;

            Assert.Multiple(() =>
            {
                Assert.That(200, Is.EqualTo(okResult.StatusCode));
                Assert.That(okResult.Value, Is.EqualTo(responseAccount));
            });

        }


        [Test]
        public void WithdrawAmount_WhenAmountGreaterThanBalance()
        {

            //Arrange
            var id = Guid.NewGuid().ToString();
            decimal amount = 3500;
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
                        Balance = 3000,
                        AccountNumber = account
                    }
                }
            };

            

            _repository.GetUser(Arg.Any<string>()).Returns(userAccount);
           
            //Act
            var result = _controller.Withdraw(id, account, amount);


            //Assert
            var badRequestResult = result as ObjectResult;

            Assert.Multiple(() =>
            {
                Assert.That(400, Is.EqualTo(badRequestResult.StatusCode));
                Assert.That(badRequestResult.Value, Is.EqualTo($"Insufficient funds - {account}"));
            });

        }

        [Test]
        public void WithdrawAmount_WhenInvalidBalanceAfterWithdrawal()
        {

            //Arrange
            var id = Guid.NewGuid().ToString();
            decimal amount = 160;
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
                        Balance = 200,
                        AccountNumber = account
                    }
                }
            };



            _repository.GetUser(Arg.Any<string>()).Returns(userAccount);

            //Act
            var result = _controller.Withdraw(id, account, amount);


            //Assert
            var badRequestResult = result as ObjectResult;

            Assert.Multiple(() =>
            {
                Assert.That(400, Is.EqualTo(badRequestResult.StatusCode));
                Assert.That(badRequestResult.Value, Is.EqualTo($"Invalid Amount. The account must have at least 100$ at any point. " +
                    $"Current amount will be {userAccount.Accounts[0].Balance - amount}$."));
            });

        }

        [Test]
        public void WithdrawAmount_WhenInvalidBalanceAfterWithdrawal_90_PercentLimit()
        {

            //Arrange
            var id = Guid.NewGuid().ToString();
            decimal amount = 2800;
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
                        Balance = 3000,
                        AccountNumber = account
                    }
                }
            };



            _repository.GetUser(Arg.Any<string>()).Returns(userAccount);
            var percentWithdrawal = (int)amount / userAccount.Accounts[0].Balance * 100;

            //Act
            var result = _controller.Withdraw(id, account, amount);


            //Assert
            var badRequestResult = result as ObjectResult;

            Assert.Multiple(() =>
            {
                Assert.That(400, Is.EqualTo(badRequestResult.StatusCode));
                Assert.That(badRequestResult.Value, Is.EqualTo($"Invalid Amount. Withdrawing more than 90% of the current balance is not allowed. " +
                    $"Current withdrawal amount is {percentWithdrawal}% of the balance."));
            });

        }
    }
}
