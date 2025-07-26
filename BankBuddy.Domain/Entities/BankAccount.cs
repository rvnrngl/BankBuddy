using BankBuddy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Domain.Entities
{
    public class BankAccount
    {
        public Guid BankAccountId { get; set; }

        public Guid UserId { get; set; }

        public string AccountNumber { get; set; } = null!;

        public decimal Balance { get; set; } = 0;

        public AccountType AccountType { get; set; }

        public AccountStatus AccountStatus { get; set; }

        public DateTime CreatedAt { get; set; }


        public User User { get; set; } = null!;
    }
}
