using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Application.DTOs.Transaction
{
    public class TransferReceiptDTO : TransactionReceipt
    {
        public AccountSummary FromAccount { get; set; } = null!;

        public AccountSummary ToAccount { get; set; } = null!;
    }
}
