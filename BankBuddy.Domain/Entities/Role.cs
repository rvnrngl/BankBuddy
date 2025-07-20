using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Domain.Entities
{
    public class Role
    {
        public Guid RoleId { get; set; }

        public string Name { get; set; } = string.Empty;

        public ICollection<User> Users { get; set; } = [];
    }
}
