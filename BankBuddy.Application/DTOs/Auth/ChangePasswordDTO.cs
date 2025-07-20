using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Application.DTOs.Auth
{
    public class ChangePasswordDTO
    {
        public string CurrentPassword { get; set; } = default!;

        public string NewPassword { get; set; } = default!;
    }
}
