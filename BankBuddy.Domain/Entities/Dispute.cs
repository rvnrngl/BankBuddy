using BankBuddy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Domain.Entities
{
    public class Dispute
    {
        public Guid DisputeId { get; set; }

        public Guid TransactionId { get; set; }

        public string Reason { get; set; } = string.Empty;

        public DisputeStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ResolvedAt { get; set; }


        public Transaction Transaction { get; set; } = null!;
    }
}
