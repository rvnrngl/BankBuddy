﻿using AutoMapper;
using BankBuddy.Application.Commons;
using BankBuddy.Application.DTOs;
using BankBuddy.Application.DTOs.BankAccount;
using BankBuddy.Application.Interfaces;
using BankBuddy.Application.Interfaces.IRepositories;
using BankBuddy.Application.Interfaces.IServices;
using BankBuddy.Domain.Entities;
using BankBuddy.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Application.Services
{
    public class BankAccountService(IGenericRepository<User> _userRepository, IGenericRepository<BankAccount> _bankAccountRepository, IAccountNumberGenerator _accountNumberGenerator, IMapper _mapper) : IBankAccountService
    {
        public async Task<BankAccountResponseDTO> CreateAccountAsync(Guid userId, AccountType type)
        {
            User? user = await _userRepository.GetByIdAsync(userId) ?? throw new AppException("User not found.", StatusCodes.Status404NotFound);

            if (!user.IsVerified) throw new AppException("User not verified yet.", StatusCodes.Status401Unauthorized);

            BankAccount account = new()
            {
                UserId = userId,
                AccountNumber = await _accountNumberGenerator.GenerateAsync(type),
                Balance = 0,
                AccountType = type,
                AccountStatus =  AccountStatus.Active,
            };

            await _bankAccountRepository.AddAsync(account);
            await _bankAccountRepository.SaveChangesAsync(); 

            return _mapper.Map<BankAccountResponseDTO>(account);
        }

        public async Task<BankAccountResponseDTO> GetUserBankAccountAsync(Guid userId, Guid bankAccountId)
        {
            BankAccount? bankAccount = await _bankAccountRepository.FindAsync(b => b.UserId == userId && b.BankAccountId == bankAccountId) ?? throw new AppException("Bank Account not found.", StatusCodes.Status404NotFound);

            return _mapper.Map<BankAccountResponseDTO>(bankAccount);
        }

        public async Task<List<BankAccountResponseDTO>> GetUserBankAccountsAsync(Guid userId, AccountType? type = null, AccountStatus? status = null)
        {
            List<BankAccount> bankAccounts = await _bankAccountRepository.FindAllAsync(
                b => b.UserId == userId &&
                (type == null || b.AccountType == type) &&
                (status == null || b.AccountStatus == status)
                );

            return _mapper.Map<List<BankAccountResponseDTO>>(bankAccounts);
        }
    }
}
