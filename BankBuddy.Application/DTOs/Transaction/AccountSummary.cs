using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Application.DTOs.Transaction
{
    public class AccountSummary
    {
        public Guid AccountId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public decimal? PreviousBalance { get; set; } // Optional for ToAccount
        public decimal? NewBalance { get; set; }      // Optional for ToAccount
    }
}
