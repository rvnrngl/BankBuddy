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
        Task<TransferReceiptDTO> TransferAsync(Guid userId, TransactionRequestDTO dto);
    }
}
