using BankingSystemAPI.Controllers;
using BankingSystemAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystemAPI.Tests.ControllersTest.TransactionControllerTests
{
    [TestFixture]
    internal class ConstructorError
    {
        private TransactionController _controller;
        private ILoggingService _logger;
        private IBankOperationsService _bankOperationsService;
        private ITransactionService _transactionService;
        [Test]
        public void ConstructorExceptionThrown_NullParamBankOperationsService()
        {
            //Act
            var ex = Assert.Throws<ArgumentNullException>(() => { _controller = new TransactionController(null, _transactionService, _logger); });

            //Assert
            Assert.That(ex, Is.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorExceptionThrown_NullParamLoggingService()
        {
            //Act
            var ex = Assert.Throws<ArgumentNullException>(() => { _controller = new TransactionController(_bankOperationsService, _transactionService, null); });

            //Assert
            Assert.That(ex, Is.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorExceptionThrown_NullParamTransactionService()
        {
            //Act
            var ex = Assert.Throws<ArgumentNullException>(() => { _controller = new TransactionController(_bankOperationsService, null, _logger); });

            //Assert
            Assert.That(ex, Is.TypeOf<ArgumentNullException>());
        }
    }
}
