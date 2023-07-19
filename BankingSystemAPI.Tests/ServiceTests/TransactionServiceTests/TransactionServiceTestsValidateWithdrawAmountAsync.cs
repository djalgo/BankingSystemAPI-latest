using BankingSystemAPI.Models;
using BankingSystemAPI.Repository;
using BankingSystemAPI.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystemAPI.Tests.ServiceTests.TransactionServiceTests
{
    [TestFixture]
    internal class TransactionServiceTestsValidateWithdrawAmountAsync
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
        public void Validate_WhenSuccessScenario()
        {
            var id = Guid.NewGuid().ToString();
            decimal amount = 200;
            var account = new Account
            {
                AccountUserId = id,
                AccountNumber = "test",
                Balance = 300,
            };
            var errors = new List<AmountValidation>();
            //Act
            var result = _service.ValidateWithdrawAmountAsync(account,
                amount).Result;
        
            //Assert
            Assert.That(result, Is.EqualTo(errors));
        }

        [Test]
        public void Validate_WhenAmountGreaterThanBalance_failure()
        {
            var id = Guid.NewGuid().ToString();
            decimal amount = 500;
            var account = new Account
            {
                AccountUserId = id,
                AccountNumber = "test",
                Balance = 300,
            };
            var error = new List<AmountValidation>()
             {
                new AmountValidation { StatusCode =400, ErrorMessage = $"Insufficient funds - {account.AccountNumber}" }
             };

            //Act

            var result = _service.ValidateWithdrawAmountAsync(account,amount).Result;

            Assert.Multiple(() => {
                //Assert
                Assert.That(result.Select(x => x.StatusCode), Is.EqualTo(error.Select(x => x.StatusCode)));
                Assert.That(result.Select(x => x.ErrorMessage), Is.EqualTo(error.Select(x => x.ErrorMessage)));
            });
        }

        [Test]
        public void Validate_WhenPercentWithdrawalGreaterThan90_failure()
        {
            var id = Guid.NewGuid().ToString();
            decimal amount = 1850;
            var account = new Account
            {
                AccountUserId = id,
                AccountNumber = "test",
                Balance = 2000,
            };
            var percent = (int) amount/account.Balance * 100;
            var error = new List<AmountValidation>()
             {
                new AmountValidation { StatusCode =400, ErrorMessage = $"Invalid Amount. Withdrawing more than 90% of the current balance is not allowed. " +
                    $"Current withdrawal amount is {percent}% of the balance." }
             };

            //Act

            var result = _service.ValidateWithdrawAmountAsync(account, amount).Result;

            Assert.Multiple(() => {
                //Assert
                Assert.That(result.Select(x => x.StatusCode), Is.EqualTo(error.Select(x => x.StatusCode)));
                Assert.That(result.Select(x => x.ErrorMessage), Is.EqualTo(error.Select(x => x.ErrorMessage)));
            });

        }

        [Test]
        public void Validate_WhenBalanceAfterWithdrawallessThan100_failure()
        {
            var id = Guid.NewGuid().ToString();
            decimal amount = 150;
            var account = new Account
            {
                AccountUserId = id,
                AccountNumber = "test",
                Balance = 200,
            };
            var percent = (int)amount / account.Balance * 100;
            var error = new List<AmountValidation>()
             {
                new AmountValidation { StatusCode =400, ErrorMessage =  $"Invalid Amount. The account must have at least 100$ at any point. " +
                    $"Current amount will be 50$." }
             };

            //Act

            var result = _service.ValidateWithdrawAmountAsync(account, amount).Result;

            Assert.Multiple(() => {
                //Assert
                Assert.That(result.Select(x => x.StatusCode), Is.EqualTo(error.Select(x => x.StatusCode)));
                Assert.That(result.Select(x => x.ErrorMessage), Is.EqualTo(error.Select(x => x.ErrorMessage)));
            });

        }

        [Test]
        public void Validate_WhenExceptionOccured_failure()
        {
            var id = Guid.NewGuid().ToString();
            decimal amount = 150;
            var account = new Account
            {
                Balance = 0
            };
            Assert.That(() => _service.ValidateWithdrawAmountAsync(account, amount).Result, Throws.Exception.TypeOf<AggregateException>());
           
        }
    }
}
