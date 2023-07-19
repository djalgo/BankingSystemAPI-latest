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
        private IBankOperationsService _service;
        private ITransactionService _transactionService;
        private ILoggingService _logger;

        [SetUp]
        public void SetUp()
        {
            //using NSubstitute for mocking here because I have experience with this framework
            _service = Substitute.For<IBankOperationsService>();
            _logger = Substitute.For<ILoggingService>();
            _transactionService = Substitute.For<ITransactionService>();
            _controller = new TransactionController(_service, _transactionService, _logger);
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

            _service.GetUserAccountAsync(Arg.Any<string>()).Returns(userAccount);
            _transactionService.DepositAmountAsync(Arg.Any<UserAccount>(), Arg.Any<Account>(), Arg.Any<decimal>()).Returns(responseAccount);
            //Act
            var result = _controller.DepositAsync(id, account, amount).Result;


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

            var accountDetail = userAccount.Accounts.Where(x => x.AccountNumber == account).FirstOrDefault();
            var validationErrors = new List<AmountValidation>
            {
               new AmountValidation
               {
                   StatusCode=400,
                   ErrorMessage =$"Insufficient funds - {account}"
               }
            };
            _service.GetUserAccountAsync(Arg.Any<string>()).Returns(userAccount);
            _transactionService.ValidateWithdrawAmountAsync(Arg.Any<Account>(), Arg.Any<decimal>()).Returns(validationErrors);
            //Act
            var result = _controller.WithdrawAsync(id, account, amount).Result;


            //Assert
            var badRequestResult = result as ObjectResult;

            Assert.Multiple(() =>
            {
                Assert.That(400, Is.EqualTo(badRequestResult.StatusCode));
                Assert.That(badRequestResult.Value, Is.EqualTo(validationErrors));
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
            var accountDetail = userAccount.Accounts.Where(x => x.AccountNumber == account).FirstOrDefault();
            var validationErrors = new List<AmountValidation>
            {
               new AmountValidation
               {
                   StatusCode=400,
                   ErrorMessage =$"Invalid Amount. The account must have at least 100$ at any point. " +
                    $"Current amount will be 40$."
               }
            };


            _service.GetUserAccountAsync(Arg.Any<string>()).Returns(userAccount);
            _transactionService.ValidateWithdrawAmountAsync(Arg.Any<Account>(), Arg.Any<decimal>()).Returns(validationErrors);

            //Act
            var result = _controller.WithdrawAsync(id, account, amount).Result;


            //Assert
            var badRequestResult = result as ObjectResult;

            Assert.Multiple(() =>
            {
                Assert.That(400, Is.EqualTo(badRequestResult.StatusCode));
                Assert.That(badRequestResult.Value, Is.EqualTo(validationErrors));
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

            var validationErrors = new List<AmountValidation>
            {
               new AmountValidation
               {
                   StatusCode=400,
                   ErrorMessage =$"The amount must be non-negative or non-zero or less than 10000$ in a single transaction."
               }
            };



            _service.GetUserAccountAsync(Arg.Any<string>()).Returns(userAccount);
            var percentWithdrawal = (int)amount / userAccount.Accounts[0].Balance * 100;
            _transactionService.ValidateWithdrawAmountAsync(Arg.Any<Account>(), Arg.Any<decimal>()).Returns(validationErrors);
            //Act
            var result = _controller.WithdrawAsync(id, account, amount).Result;


            //Assert
            var badRequestResult = result as ObjectResult;

            Assert.Multiple(() =>
            {
                Assert.That(400, Is.EqualTo(badRequestResult.StatusCode));
                Assert.That(badRequestResult.Value, Is.EqualTo(validationErrors));
            });

        }
    }
}
