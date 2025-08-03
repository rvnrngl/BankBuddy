using BankBuddy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Application.DTOs.Transaction
{
    public class TransactionHistoryDTO
    {
        public Guid TransactionId { get; set; }

        public string? ReferenceId { get; set; }

        public Guid? FromAccountId { get; set; }
        public string? FromAccountNumber { get; set; }
        public string? FromAccountName { get; set; }

        public Guid? ToAccountId { get; set; }
        public string? ToAccountNumber { get; set; }
        public string? ToAccountName { get; set; }

        public decimal Amount { get; set; }

        public string? Description { get; set; }

        public TransactionType Type { get; set; }

        public TransactionStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
