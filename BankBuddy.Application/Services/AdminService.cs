using AutoMapper;
using BankBuddy.Application.Commons;
using BankBuddy.Application.DTOs.Auth;
using BankBuddy.Application.DTOs.BankAccount;
using BankBuddy.Application.Interfaces.IRepositories;
using BankBuddy.Application.Interfaces.IServices;
using BankBuddy.Domain.Entities;
using BankBuddy.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Application.Services
{
    public class AdminService(IGenericRepository<User> _userRepository, IGenericRepository<Role> _roleRepository, IGenericRepository<BankAccount> _bankAccountRepository, IMapper _mapper) : IAdminService
    {
        #region UserModule
        
        public async Task<string> CreateAdminAsync(RegisterDTO dto)
        {
            User? existingUser = await _userRepository.FindAsync(u => u.Email == dto.Email);

            if (existingUser is not null) throw new AppException("Email already in use.", StatusCodes.Status409Conflict);

            Role? userRole = await _roleRepository.FindAsync(r => r.Name == "Admin") ?? throw new AppException("Administrator role not found.", StatusCodes.Status404NotFound);

            var user = _mapper.Map<User>(dto);

            user.RoleId = userRole.RoleId;

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return "Admin created successfully.";
        }

        public async Task<List<UserDetailsDTO>> GetUsersAsync(string? role = null, bool? isVerified = null)
        {
            List<User> users = await _userRepository.FindAllAsync(u =>
                (string.IsNullOrEmpty(role) || u.Role.Name == role) &&
                (isVerified == null || u.IsVerified == isVerified),
                include: q => q.Include(u => u.Role)
            );

            return _mapper.Map<List<UserDetailsDTO>>(users);
        }

        #endregion

        #region BankAccountModule

        public async Task<List<BankAccountResponseDTO>> GetBankAccountsAsync(Guid? userId = null, AccountType? type = null, AccountStatus? status = null)
        {
            List<BankAccount> bankAccounts = await _bankAccountRepository.FindAllAsync(b =>
                (userId == null || b.UserId == userId) &&
                (type == null || b.AccountType == type) &&
                (status == null || b.AccountStatus == status)
                );

            return _mapper.Map<List<BankAccountResponseDTO>>(bankAccounts);
        }

        public async Task<string> UpdateAccountStatusAsync(Guid accountId, AccountStatus newStatus)
        {
            BankAccount? account = await _bankAccountRepository.GetByIdAsync(accountId) ?? throw new AppException("Bank Account not found.", StatusCodes.Status404NotFound);

            if(account.AccountStatus == newStatus) throw new AppException("Account already in the specified status.", StatusCodes.Status400BadRequest);

            if(account.AccountStatus == AccountStatus.Closed) throw new AppException("Cannot modify a closed account", StatusCodes.Status400BadRequest);

            account.AccountStatus = newStatus;
            _bankAccountRepository.Update(account);
            await _bankAccountRepository.SaveChangesAsync();

            return "Account status updated successfully";
        }

        #endregion

    }
}
