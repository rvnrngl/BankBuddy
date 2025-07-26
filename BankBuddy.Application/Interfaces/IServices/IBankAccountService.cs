using BankBuddy.Application.DTOs.BankAccount;
using BankBuddy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Application.Interfaces.IServices
{
    public interface IBankAccountService
    {
        Task<BankAccountResponseDTO> CreateAccountAsync(Guid userId, AccountType type);
        Task<BankAccountResponseDTO> GetUserBankAccountAsync(Guid userId, Guid bankAccountId);
        Task<List<BankAccountResponseDTO>> GetUserBankAccountsAsync(Guid userId, AccountType? type = null, AccountStatus? status = null);
    }
}
