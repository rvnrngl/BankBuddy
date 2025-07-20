using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Domain.Entities
{
    public class User
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; } = null!;

        public string MiddleName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public bool IsVerified { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid RoleId { get; set; }


        public Role Role { get; set; } = null!;

        public ICollection<RefreshToken> RefreshTokens { get; set; } = [];

    }
}
