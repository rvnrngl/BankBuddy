using BankBuddy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Application.DTOs.Auth
{
    public class AuthData
    {
        public User User { get; set; } = default!;

        public Role Role { get; set; } = default!;

        public string AccessToken { get; set; } = default!;

        public string RefreshToken { get; set; } = default!;
    }
}
