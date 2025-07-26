using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Domain.Entities
{
    public class VerificationToken
    {
        public Guid VerificationTokenId { get; set; }

        public Guid UserId { get; set; }

        public string Token { get; set; } = null!;

        public DateTime ExpiresAt { get; set; }


        public User User { get; set; } = null!;
    }
}
