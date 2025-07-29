using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Application.DTOs.Transaction
{
    public class WithdrawRequestDTO
    {
        [Required]
        public Guid FromAccountId { get; set; }

        [Range(100.00, double.MaxValue, ErrorMessage = "Minimum withdrawal is ₱100.")]
        public decimal Amount { get; set; }

        public string? Description { get; set; }
    }
}
