using BankingSystemAPI.Controllers;
using BankingSystemAPI.Models;
using BankingSystemAPI.Repository;
using BankingSystemAPI.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystemAPI.Tests.ControllersTest.BankOperationsControllerTests
{
    [TestFixture]
    internal class BankOperationsControllerTests
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
        public void GetUserAccounts_ReturnsUserAccounts_WhenExists()
        {

            //Arrange
            var id = Guid.NewGuid().ToString();
            var account = Guid.NewGuid().ToString();
            var userAccounts = new List<UserAccount> {
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
                            Balance = 200,
                            CreatedDate = DateTime.Now
                        }
                    }
                }

            };

            _repository.GetUsers().Returns(userAccounts);
            //Act
            var result = _controller.GetUserAccounts();

            //Assert
            Assert.That(result, Is.EqualTo(userAccounts));
            Assert.That(result.Count, Is.EqualTo(1));

        }

        [Test]
        public void GetUserAccounts_ReturnsNull_WhenNoneExists()
        {

            //Arrange
            List<UserAccount> userAccounts = null;
            
                

            _repository.GetUsers().Returns(userAccounts);
            //Act
            var result = _controller.GetUserAccounts();

            //Assert
            Assert.That(result, Is.EqualTo(null));
      

        }
    }
}
