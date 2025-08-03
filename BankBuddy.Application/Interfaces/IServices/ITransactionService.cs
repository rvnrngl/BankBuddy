using BankBuddy.Application.Commons.Utils;
using BankBuddy.Application.DTOs.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Application.Interfaces.IServices
{
    public interface ITransactionService
    {
        Task<DepositReceiptDTO> DepositAsync(Guid userId, DepositRequestDTO dto);
        Task<PaginatedResult<TransactionHistoryDTO>> GetMyTransactionHistoryAsync(Guid userId, TransactionHistoryQueryDTO query);
        Task<TransferReceiptDTO> TransferAsync(Guid userId, TransactionRequestDTO dto);
        Task<WithdrawReceiptDTO> WithdrawAsync(Guid userId, WithdrawRequestDTO dto);
    }
}
