using AutoMapper;
using AutoMapper.QueryableExtensions;
using BankBuddy.Application.Commons;
using BankBuddy.Application.Commons.Utils;
using BankBuddy.Application.DTOs.Transaction;
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
using System.Transactions;
using Transaction = BankBuddy.Domain.Entities.Transaction;
using TransactionStatus = BankBuddy.Domain.Enums.TransactionStatus;

namespace BankBuddy.Application.Services
{
    public class TransactionService(IGenericRepository<BankAccount> _bankAccountRepository, IGenericRepository<Transaction> _transactionRepository, IMapper _mapper) : ITransactionService
    {
        public async Task<TransferReceiptDTO> TransferAsync(Guid userId, TransactionRequestDTO dto)
        {
            if (dto.FromAccountId == dto.ToAccountId)
                throw new AppException("Cannot transfer to the same account.", StatusCodes.Status400BadRequest);

            using TransactionScope scope = new(TransactionScopeAsyncFlowOption.Enabled);

            BankAccount fromAccount = await _bankAccountRepository.FindAsync(
                b => b.BankAccountId == dto.FromAccountId && b.UserId == userId,
                include: q => q.Include(b => b.User))
                ?? throw new AppException("Invalid source account.", StatusCodes.Status403Forbidden);

            BankAccount toAccount = await  _bankAccountRepository.FindAsync(b => b.BankAccountId == dto.ToAccountId)
                ?? throw new AppException("Destination account not found.", StatusCodes.Status404NotFound);

            if (fromAccount.Balance < dto.Amount)
                throw new AppException("Insufficient funds.", StatusCodes.Status409Conflict);

            bool isInternal = fromAccount.UserId == toAccount.UserId;

            decimal fromPrevBalance = fromAccount.Balance;
            decimal toPrevBalance = toAccount.Balance;

            fromAccount.Balance -= dto.Amount;
            toAccount.Balance += dto.Amount;

            string referenceId = $"TXN-{DateTime.UtcNow:yyyyMMddHHmmssfff}";

            Transaction transaction = new()
            {
                FromAccountId = fromAccount.BankAccountId,
                ToAccountId = toAccount.BankAccountId,
                Amount = dto.Amount,
                Description = dto.Description,
                Type = isInternal ? TransactionType.TransferInternal : TransactionType.TransferExternal,
                Status = TransactionStatus.Completed,
                CreatedAt = DateTime.UtcNow,
                ReferenceId = referenceId
            };

            await _transactionRepository.AddAsync(transaction);
            await _transactionRepository.SaveChangesAsync();

            _bankAccountRepository.Update(fromAccount);
            _bankAccountRepository.Update(toAccount);
            await _bankAccountRepository.SaveChangesAsync();

            scope.Complete();

            TransferReceiptDTO receipt = _mapper.Map<TransferReceiptDTO>(transaction);
            receipt.ReferenceId = referenceId;

            receipt.FromAccount = _mapper.Map<AccountSummary>(fromAccount);
            receipt.ToAccount = _mapper.Map<AccountSummary>(toAccount);

            receipt.FromAccount.PreviousBalance = fromPrevBalance;
            receipt.FromAccount.NewBalance = fromAccount.Balance;

            receipt.ToAccount.PreviousBalance = toPrevBalance;
            receipt.ToAccount.NewBalance = toAccount.Balance;

            return receipt;
        }

        public async Task<DepositReceiptDTO> DepositAsync(Guid userId, DepositRequestDTO dto)
        {
            using TransactionScope scope = new(TransactionScopeAsyncFlowOption.Enabled);

            if (dto.Amount < 50) throw new AppException("Amount must be at least ₱50.00.", StatusCodes.Status400BadRequest);

            BankAccount account = await _bankAccountRepository.FindAsync(
                b => b.BankAccountId == dto.ToAccountId && b.UserId == userId,
                include: q => q.Include(b => b.User))
                ?? throw new AppException("Account not found.", StatusCodes.Status404NotFound);

            decimal prevBalance = account.Balance;
            account.Balance += dto.Amount;

            Transaction transaction = new()
            {
                ToAccountId = account.BankAccountId,
                Amount = dto.Amount,
                Description = dto.Description,
                Type = TransactionType.Deposit,
                Status = TransactionStatus.Completed,
                CreatedAt = DateTime.UtcNow,
                ReferenceId = $"DEP-{DateTime.UtcNow:yyyyMMddHHmmssfff}"
            };

            await _transactionRepository.AddAsync(transaction);
            await _transactionRepository.SaveChangesAsync();

            _bankAccountRepository.Update(account);
            await _bankAccountRepository.SaveChangesAsync();

            scope.Complete();

            var receipt = _mapper.Map<DepositReceiptDTO>(transaction);
            receipt.ToAccount = _mapper.Map<AccountSummary>(account);
            receipt.ToAccount.PreviousBalance = prevBalance;
            receipt.ToAccount.NewBalance = account.Balance;
            receipt.ReferenceId = transaction.ReferenceId;

            return receipt;
        }

        public async Task<WithdrawReceiptDTO> WithdrawAsync(Guid userId, WithdrawRequestDTO dto)
        {
            using TransactionScope scope = new(TransactionScopeAsyncFlowOption.Enabled);

            if (dto.Amount < 100) throw new AppException("Amount must be at least ₱100.00.", StatusCodes.Status400BadRequest);

            BankAccount account = await _bankAccountRepository.FindAsync(
                b => b.BankAccountId == dto.FromAccountId && b.UserId == userId,
                include: q => q.Include(b => b.User))
                ?? throw new AppException("Account not found.", StatusCodes.Status404NotFound);

            if (account.Balance < dto.Amount)
                throw new AppException("Insufficient funds.", StatusCodes.Status409Conflict);

            decimal prevBalance = account.Balance;
            account.Balance -= dto.Amount;

            Transaction transaction = new()
            {
                FromAccountId = account.BankAccountId,
                Amount = dto.Amount,
                Description = dto.Description,
                Type = TransactionType.Withdrawal,
                Status = TransactionStatus.Completed,
                CreatedAt = DateTime.UtcNow,
                ReferenceId = $"WTH-{DateTime.UtcNow:yyyyMMddHHmmssfff}"
            };

            await _transactionRepository.AddAsync(transaction);
            await _transactionRepository.SaveChangesAsync();

            _bankAccountRepository.Update(account);
            await _bankAccountRepository.SaveChangesAsync();

            scope.Complete();

            var receipt = _mapper.Map<WithdrawReceiptDTO>(transaction);
            receipt.FromAccount = _mapper.Map<AccountSummary>(account);
            receipt.FromAccount.PreviousBalance = prevBalance;
            receipt.FromAccount.NewBalance = account.Balance;
            receipt.ReferenceId = transaction.ReferenceId;

            return receipt;
        }

        public async Task<PaginatedResult<TransactionHistoryDTO>> GetMyTransactionHistoryAsync(Guid userId, TransactionHistoryQueryDTO query)
        {
            BankAccount? account = await _bankAccountRepository.FindAsync(
                b => b.BankAccountId == query.AccountId && b.UserId == userId)
                ?? throw new AppException("Account not found or does not belong to you.", StatusCodes.Status403Forbidden);

            IQueryable<Transaction> transactions = _transactionRepository.Query()
                .Include(t => t.FromAccount).ThenInclude(a => a.User)
                .Include(t => t.ToAccount).ThenInclude(a => a.User)
                .Where(t =>
                    (
                        (t.FromAccount != null && t.FromAccount.UserId == userId) ||
                        (t.ToAccount != null && t.ToAccount.UserId == userId)
                    ) &&
                    (t.FromAccountId == query.AccountId || t.ToAccountId == query.AccountId) &&
                    (!query.Type.HasValue || t.Type == query.Type) &&
                    (!query.FromDate.HasValue || t.CreatedAt >= query.FromDate) &&
                    (!query.ToDate.HasValue || t.CreatedAt <= query.ToDate) &&
                    (!query.MinAmount.HasValue || t.Amount >= query.MinAmount) &&
                    (!query.MaxAmount.HasValue || t.Amount <= query.MaxAmount)
                );

            int totalItems = await transactions.CountAsync();

            List<Transaction> transactionList = await transactions
                .OrderByDescending(t => t.CreatedAt)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            List<TransactionHistoryDTO> results = _mapper.Map<List<TransactionHistoryDTO>>(transactionList);

            return new PaginatedResult<TransactionHistoryDTO>(results, query.Page, query.PageSize, totalItems);
        }
    }
}
