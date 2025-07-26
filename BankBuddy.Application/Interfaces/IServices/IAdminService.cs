using BankBuddy.Application.DTOs.Auth;
using BankBuddy.Application.DTOs.BankAccount;
using BankBuddy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Application.Interfaces.IServices
{
    public interface IAdminService
    {
        Task<string> CreateAdminAsync(RegisterDTO dto);
        Task<List<BankAccountResponseDTO>> GetBankAccountsAsync(Guid? userId = null, AccountType? type = null, AccountStatus? status = null);
        Task<List<UserDetailsDTO>> GetUsersAsync(string? role = null, bool? isVerified = null);
        Task<string> UpdateAccountStatusAsync(Guid accountId, AccountStatus newStatus);
    }
}
