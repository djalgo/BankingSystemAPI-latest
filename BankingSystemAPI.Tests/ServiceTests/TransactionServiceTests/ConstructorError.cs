using BankingSystemAPI.Controllers;
using BankingSystemAPI.Repository;
using BankingSystemAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystemAPI.Tests.ServiceTests.TransactionServiceTests
{
    [TestFixture]
    internal class ConstructorError
    {
        private TransactionService _service;
        private ILoggingService _logger;
        private IBankingOperationsRepository _repository;
        
        [Test]
        public void ConstructorExceptionThrown_NullParamRepository()
        {
            //Act
            var ex = Assert.Throws<ArgumentNullException>(() => { _service = new TransactionService(null, _logger); });

            //Assert
            Assert.That(ex, Is.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorExceptionThrown_NullParamLoggingService()
        {
            //Act
            var ex = Assert.Throws<ArgumentNullException>(() => { _service = new TransactionService(_repository, null); });

            //Assert
            Assert.That(ex, Is.TypeOf<ArgumentNullException>());
        }
    }
}
