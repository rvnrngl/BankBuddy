using BankBuddy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Application.Interfaces
{
    public interface IAccountNumberGenerator
    {
        Task<string> GenerateAsync(AccountType accountType);
    }
}
