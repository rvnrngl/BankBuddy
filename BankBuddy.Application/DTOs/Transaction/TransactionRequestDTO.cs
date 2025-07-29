using BankBuddy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Application.DTOs.Transaction
{
    public class TransactionRequestDTO
    {
        public Guid? FromAccountId { get; set; }

        public Guid? ToAccountId { get; set; }

        public decimal Amount { get; set; }

        public string? Description { get; set; }
    }
}
