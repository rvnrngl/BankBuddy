using BankBuddy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Domain.Entities
{
    public class Transaction
    {
        public Guid TransactionId { get; set; }

        public Guid? FromAccountId { get; set; }

        public Guid? ToAccountId { get; set; }

        public decimal Amount { get; set; }

        public string? Description { get; set; }

        public TransactionType Type { get; set; }

        public TransactionStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }


        public BankAccount? FromAccount { get; set; }

        public BankAccount? ToAccount { get; set; }


        public string? ReferenceId { get; set; }
    }
}
