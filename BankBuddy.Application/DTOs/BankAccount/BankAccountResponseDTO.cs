using BankBuddy.Domain.Entities;
using BankBuddy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Application.DTOs.BankAccount
{
    public class BankAccountResponseDTO
    {
        public Guid BankAccountId { get; set; }

        public string AccountNumber { get; set; } = default!;

        public decimal Balance { get; set; }

        public AccountType AccountType { get; set; }

        public AccountStatus AccountStatus { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
