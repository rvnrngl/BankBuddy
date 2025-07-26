using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Application.DTOs.Auth
{
    public class UserInfoDTO
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; } = default!;

        public string MiddleName { get; set; } = default!;

        public string LastName { get; set; } = default!;

        public string Email { get; set; } = default!;
        
        public bool IsVerified { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
