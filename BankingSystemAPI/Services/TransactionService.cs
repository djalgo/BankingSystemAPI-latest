﻿using BankingSystemAPI.Models;
using BankingSystemAPI.Repository;
using System.Net;

namespace BankingSystemAPI.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IBankingOperationsRepository _bankingOperationsRepository;
        private readonly ILoggingService _logger;

        /// <summary>
        /// TransactionService Constructor
        /// </summary>
        /// <param name="bankingOperationsRepository"></param>
        /// <param name="logger"></param>
        public TransactionService(IBankingOperationsRepository bankingOperationsRepository, ILoggingService logger) {
            _bankingOperationsRepository = bankingOperationsRepository ?? throw new ArgumentNullException(nameof(bankingOperationsRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<Account> DepositAmountAsync(UserAccount user, Account account, decimal amount)
        {
            try
            {
                return await _bankingOperationsRepository.DepositAmountAsync(user, account, amount);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Account> WithdrawAmountAsync(UserAccount user, Account account, decimal amount)
        {
            try
            {
                return await _bankingOperationsRepository.WithdrawAmountAsync(user, account, amount);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<AmountValidation>> ValidateWithdrawAmountAsync(Account account, decimal amount)
        {
            var errors = new List<AmountValidation>();
            try
            {
                var balance = account.Balance;
                var balanceAfterWithdrawal = balance - amount;
                var percentWithdrawal = (int)amount / balance * 100;
                if (amount > balance)
                {
                    _logger.LogError($"Insufficient funds - {account.AccountNumber}");
                    errors.Add(new AmountValidation
                    {
                        StatusCode = 400,
                        ErrorMessage = $"Insufficient funds - {account.AccountNumber}"
                    });
                    return errors;
                }
                
                if (percentWithdrawal > 90)
                {
                    _logger.LogError($"Invalid Amount. Withdrawing more than 90% of the current balance is not allowed. " +
                        $"Current withdrawal amount is {percentWithdrawal}% of the balance.");
                    errors.Add(new AmountValidation
                    {
                        StatusCode = 400,
                        ErrorMessage = $"Invalid Amount. Withdrawing more than 90% of the current balance is not allowed. " +
                        $"Current withdrawal amount is {percentWithdrawal}% of the balance."
                    });

                }

                if (balanceAfterWithdrawal < 100)
                {
                    _logger.LogError($"Invalid Amount. The account must have at least 100$ at any point. " +
                        $"Current amount will be {balanceAfterWithdrawal}$.");
                    errors.Add(new AmountValidation
                    {
                        StatusCode = 400,
                        ErrorMessage = $"Invalid Amount. The account must have at least 100$ at any point. " +
                        $"Current amount will be {balanceAfterWithdrawal}$."
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return errors;
        }

        public async Task<IEnumerable<AmountValidation>> ValidateDepositAmountAsync(decimal amount)
        {
            var error = new List<AmountValidation>();
            try
            {
                if (amount > 10000)
                {
                    _logger.LogError($"The amount must be less than 10000$ in a single transaction.");
                    error.Add(new AmountValidation { StatusCode = 400, ErrorMessage = $"The amount must be less than 10000$ in a single transaction." });
                }

                if (amount <= 0)
                {
                    _logger.LogError($"The amount must be non-negative and non-zero.");
                    error.Add(new AmountValidation { StatusCode = 400, ErrorMessage = $"The amount must be non-negative and non-zero." });
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException(nameof(ex));
            }
            return error;

        }
    }
}
