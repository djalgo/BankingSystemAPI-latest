using BankingSystemAPI.Controllers;
using BankingSystemAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystemAPI.Tests.ControllersTest.BankOperationsControllerTests
{
    [TestFixture]
    internal class ConstructorError
    {
        private BankOperationsController _controller;
        private ILoggingService _logger;
        private IBankOperationsService _bankOperationsService;
        [Test]
        public void ConstructorExceptionThrown_NullParamBankOperationsService() 
        { 
            //Act
            var ex = Assert.Throws<ArgumentNullException>(() => { _controller = new BankOperationsController(null, _logger); });

            //Assert
            Assert.That(ex, Is.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorExceptionThrown_NullParamLoggingService()
        {
            //Act
            var ex = Assert.Throws<ArgumentNullException>(() => { _controller = new BankOperationsController(_bankOperationsService, null); });

            //Assert
            Assert.That(ex, Is.TypeOf<ArgumentNullException>());
        }
    }
}
