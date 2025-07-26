using BankBuddy.Application.Interfaces;
using BankBuddy.Application.Interfaces.IRepositories;
using BankBuddy.Domain.Entities;
using BankBuddy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Application.Commons.Utils
{
    /// <summary>
    /// Generates unique and Luhn-valid bank account numbers based on account type.
    /// </summary>
    public class AccountNumberGenerator(IGenericRepository<BankAccount> _bankAccountRepository) : IAccountNumberGenerator
    {
        /// <summary>
        /// Generates a unique and valid account number for the given account type.
        /// Format:
        /// - Prefix based on account type (e.g., "10" for Savings, "20" for Checking)
        /// - Random numeric digits to match the required total length
        /// - A final Luhn check digit to ensure validity
        /// </summary>
        public async Task<string> GenerateAsync(AccountType accountType)
        {
            int totalLength = accountType == AccountType.Savings ? 12 : 20;
            string prefix = GetPrefix(accountType);
            int randomLength = totalLength - prefix.Length - 1;

            string baseNumber;
            string fullAccountNumber;

            do
            {
                string randomDigits = GenerateRandomDigits(randomLength);
                baseNumber = prefix + randomDigits;
                int checkDigit = CalculateLuhnCheckDigit(baseNumber);
                fullAccountNumber = baseNumber + checkDigit;
            }
            while (await _bankAccountRepository.AnyAsync(a => a.AccountNumber == fullAccountNumber));

            return fullAccountNumber;
        }

        /// <summary>
        /// Returns a fixed prefix for the given account type.
        /// </summary>
        private static string GetPrefix(AccountType type)
        {
            return type switch
            {
                AccountType.Savings => "10",
                AccountType.Checking => "20",
                _ => "00" // fallback
            };
        }

        /// <summary>
        /// Generates a string of random numeric digits of the specified length.
        /// </summary>
        private static string GenerateRandomDigits(int length)
        {
            Random rnd = new();
            return new string([.. Enumerable.Range(0, length).Select(_ => (char)('0' + rnd.Next(0, 10)))]);
        }

        /// <summary>
        /// Calculates the Luhn check digit for the given numeric string.
        /// </summary>
        private static int CalculateLuhnCheckDigit(string number)
        {
            int sum = 0;
            bool doubleDigit = true;

            for (int i = number.Length - 1; i >= 0; i--)
            {
                int digit = number[i] - '0';

                if (doubleDigit)
                {
                    digit *= 2;
                    if (digit > 9)
                        digit -= 9;
                }

                sum += digit;
                doubleDigit = !doubleDigit;
            }

            return (10 - (sum % 10)) % 10;
        }
    }
}
