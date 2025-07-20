using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Application.DTOs.Auth
{
    public class AuthResponseDTO
    {
        public Guid UserId { get; set; }

        public string FullName { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string Role { get; set; } = default!;

        public string AccessToken { get; set; } = default!;

        public string RefreshToken { get; set; } = default!;
    }
}
