using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Application.DTOs.Transaction
{
    public class DepositRequestDTO
    {
        public Guid ToAccountId { get; set; }

        [Range(50.00, double.MaxValue, ErrorMessage = "Minimum deposit is ₱50.")]
        public decimal Amount { get; set; }

        public string? Description { get; set; }

    }
}
