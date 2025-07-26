using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Application.DTOs.Auth
{
    public class UserDetailsDTO : UserInfoDTO
    {
        public string Role { get; set; }
    }
}
