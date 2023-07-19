using BankingSystemAPI.Models;
using BankingSystemAPI.Repository;
using BankingSystemAPI.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystemAPI.Tests.ServiceTests.TransactionServiceTests
{
    [TestFixture]
    internal class TransactionServiceTestsValidateDepositAmountAsync
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
            var errors = new List<AmountValidation>();
            //Act
            var result = _service.ValidateDepositAmountAsync(amount).Result;
        
            //Assert
            Assert.That(result, Is.EqualTo(errors));
        }

        [Test]
        public void Validate_WhenAmountGreaterThan10000_failure()
        {
            decimal amount = 11000;
            var error = new List<AmountValidation>()
             {
                new AmountValidation { StatusCode =400, ErrorMessage = $"The amount must be less than 10000$ in a single transaction." }
             };

            //Act

            var result = _service.ValidateDepositAmountAsync(amount).Result;

            Assert.Multiple(() => {
                //Assert
                Assert.That(result.Select(x => x.StatusCode), Is.EqualTo(error.Select(x => x.StatusCode)));
                Assert.That(result.Select(x => x.ErrorMessage), Is.EqualTo(error.Select(x => x.ErrorMessage)));
            });
        }

        [Test]
        public void Validate_WhenAmountLessThanEqualZero_failure()
        {
            decimal amount = 0;
            var error = new List<AmountValidation>()
             {
                new AmountValidation { StatusCode = 400, ErrorMessage =$"The amount must be non-negative and non-zero." }
             };

            //Act

            var result = _service.ValidateDepositAmountAsync(amount).Result;

            //Assert
            Assert.Multiple(() => {
                //Assert
                Assert.That(result.Select(x => x.StatusCode), Is.EqualTo(error.Select(x => x.StatusCode)));
                Assert.That(result.Select(x => x.ErrorMessage), Is.EqualTo(error.Select(x => x.ErrorMessage)));
            });

        }

        [Test]
        public void Validate_WhenExceptionoccured_failure()
        {
            decimal amount = -1;
            var error = new List<AmountValidation>()
             {
                new AmountValidation { StatusCode = 400, ErrorMessage =$"The amount must be non-negative and non-zero." }
             };

            //Act

            var result = _service.ValidateDepositAmountAsync(amount).Result;

            //Assert
            Assert.Multiple(() => {
                //Assert
                Assert.That(result.Select(x => x.StatusCode), Is.EqualTo(error.Select(x => x.StatusCode)));
                Assert.That(result.Select(x => x.ErrorMessage), Is.EqualTo(error.Select(x => x.ErrorMessage)));
            });

        }

    }
}
