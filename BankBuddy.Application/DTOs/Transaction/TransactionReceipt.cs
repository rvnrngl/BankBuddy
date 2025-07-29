using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Application.DTOs.Transaction
{
    public class TransactionReceipt
    {
        public Guid TransactionId { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; }

        public string ReferenceId { get; set; } = string.Empty;
    }
}
