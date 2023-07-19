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
    internal class TransactionControllerTestsDepositAmount
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
        public void DepositAmount_WhenSuccessful()
        {

            //Arrange
            var id = Guid.NewGuid().ToString();
            decimal amount = 200;
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

            var newAccountNumber = Guid.NewGuid().ToString();
            var responseAccount = new Account
            {

                AccountUserId = id,
                Balance = 300,
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
        public void DepositAmount_WhenAmountInvalid_Failure()
        {

            //Arrange
            var id = Guid.NewGuid().ToString();
            decimal amount1 = 0;
            decimal amount2 = 11000;
            
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
            var validationErrorsZeroAmount = new List<AmountValidation>
            {
               new AmountValidation
               {
                   StatusCode=400,
                   ErrorMessage =$"The amount must be non-negative and non-zero."
               }
            };

            var validationErrorsGreaterThan10000 = new List<AmountValidation>
            {
               new AmountValidation
               {
                   StatusCode=400,
                   ErrorMessage =$"The amount must be non-negative or non-zero or less than 10000$ in a single transaction."
               }
            };

            _service.GetUserAccountAsync(Arg.Any<string>()).Returns(userAccount);
            _transactionService.ValidateDepositAmountAsync(amount1).Returns(validationErrorsZeroAmount);
            _transactionService.ValidateDepositAmountAsync(amount2).Returns(validationErrorsGreaterThan10000);
            //Act
            var result_Zero_Amount = _controller.DepositAsync(id, account, amount1).Result;
            var result_greaterThan_10000 = _controller.DepositAsync(id, account, amount2).Result;


            //Assert
            var badRequestResult_Zero_Amount = result_Zero_Amount as ObjectResult;
            var badRequestresult_greaterThan_10000 = result_greaterThan_10000 as ObjectResult;

            Assert.Multiple(() =>
            {
                Assert.That(400, Is.EqualTo(badRequestResult_Zero_Amount.StatusCode));
                Assert.That(badRequestResult_Zero_Amount.Value, Is.EqualTo(validationErrorsZeroAmount));
                Assert.That(400, Is.EqualTo(badRequestresult_greaterThan_10000.StatusCode));
                Assert.That(badRequestresult_greaterThan_10000.Value, Is.EqualTo(validationErrorsGreaterThan10000));

            });

        }
    }
}
