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
    internal class BankOperationServiceTestsAddNewUserAccount
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
        public void CreateNewUserAccount_ReturnsUserAccounts_WhenCreated()
        {

            //Arrange
            var id = Guid.NewGuid().ToString();
            var account = Guid.NewGuid().ToString();
            var userAccount = new UserAccountDto
            {
                FirstName = "test f name",
                LastName = "test l name",
                Email = "test-email",
                Accounts = new List<AccountDto> { 
                    new AccountDto
                    {
                        Balance = 100,
                    }
                }
            };
            var userAccountResponse =
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
                            Balance = 100,
                            CreatedDate = DateTime.Now
                        }
                    }
                };

            _repository.AddUserAsync(Arg.Any<UserAccount>()).Returns(userAccountResponse);
            //Act
            var result = _service.AddNewUserAccountAsync(userAccount).Result;

            //Assert
            
            Assert.That(result, Is.EqualTo(userAccountResponse));
            
            
        }

    }
}
