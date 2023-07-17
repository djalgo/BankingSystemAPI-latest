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

            _repository.GetUser(Arg.Any<string>()).Returns(userAccount);
            
            //Act
            var result_Zero_Amount = _controller.Deposit(id, account, amount1);
            var result_greaterThan_10000 = _controller.Deposit(id, account, amount2);


            //Assert
            var badRequestResult_Zero_Amount = result_Zero_Amount as ObjectResult;
            var badRequestresult_greaterThan_10000 = result_greaterThan_10000 as ObjectResult;

            Assert.Multiple(() =>
            {
                Assert.That(400, Is.EqualTo(badRequestResult_Zero_Amount.StatusCode));
                Assert.That(badRequestResult_Zero_Amount.Value, Is.EqualTo($"The amount must be non-negative or non-zero or less than 10000$ in a single transaction."));
                Assert.That(400, Is.EqualTo(badRequestresult_greaterThan_10000.StatusCode));
                Assert.That(badRequestresult_greaterThan_10000.Value, Is.EqualTo($"The amount must be non-negative or non-zero or less than 10000$ in a single transaction."));
                _logger.Received(2).LogError($"The amount must be non-negative or non-zero or less than 10000$ in a single transaction.");


            });

        }
    }
}
