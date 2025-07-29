using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Application.DTOs.Transaction
{
    public class WithdrawReceiptDTO : TransactionReceipt
    {
        public AccountSummary FromAccount { get; set; } = null!;
    }
}
