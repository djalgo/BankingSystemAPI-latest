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

namespace BankingSystemAPI.Tests.ControllersTest.BankOperationsServiceTests
{
    [TestFixture]
    internal class BankOperationsServicesTestsDeleteAccountForUser
    {
        private BankOperationsService _service;
        private IBankingOperationsRepository _repository;
        private ILoggingService _logger;

        [SetUp]
        public void SetUp()
        {
            //using NSubstitute for mocking here because I have experience with this framework
            _repository = Substitute.For<IBankingOperationsRepository>();
            _logger = Substitute.For<ILoggingService>();
            _service = new BankOperationsService(_repository, _logger);
        }

        [Test]
        public void DeleteAccount_ReturnsVoid_WhenSuccessful()
        {

            //Arrange
            var id = Guid.NewGuid().ToString();
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
            
            _repository.DeleteAccountForUserAsync(Arg.Any<UserAccount>(), Arg.Any<string>());
            //Act
            _service.DeleteAccountForUserAsync(userAccount, account);

            //Assert
            _logger.Received(1).LogInformation($"Account deleted - {account}");
        }
    }
}
