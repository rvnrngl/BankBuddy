using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Application.DTOs.Auth
{
    public class LoginDTO
    {
        public string Email { get; set; } = default!;

        public string Password { get; set; } = default!;
    }
}
