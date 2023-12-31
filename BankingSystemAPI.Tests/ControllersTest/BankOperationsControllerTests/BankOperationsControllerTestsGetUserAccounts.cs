﻿using BankingSystemAPI.Controllers;
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
    internal class BankOperationsControllerTests
    {
        private BankOperationsController _controller;
        private IBankOperationsService _service;
        private ILoggingService _logger;

        [SetUp]
        public void SetUp()
        {
            //using NSubstitute for mocking here because I have experience with this framework
            _service = Substitute.For<IBankOperationsService>();
            _logger = Substitute.For<ILoggingService>();
            _controller = new BankOperationsController(_service, _logger);
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

            _service.GetUserAccountsAsync().Returns(userAccounts);
            //Act
            var result = _controller.GetUserAccountsAsync().Result;
            var okResult = result as ObjectResult;


            //Assert
            Assert.That(okResult.Value, Is.EqualTo(userAccounts));
            
        }

        [Test]
        public void GetUserAccounts_ReturnsNull_WhenNoneExists()
        {

            //Arrange
            List<UserAccount> userAccounts = new List<UserAccount>();


            _service.GetUserAccountsAsync().Returns(userAccounts);
            //Act
            var result = _controller.GetUserAccountsAsync().Result;
            var notFoundResult = result as ObjectResult;
            //Assert
            Assert.That(notFoundResult.Value, Is.EqualTo(userAccounts));
        }
    }
}
