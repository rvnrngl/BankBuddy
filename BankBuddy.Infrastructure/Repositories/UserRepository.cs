using BankBuddy.Application.Interfaces.IRepositories;
using BankBuddy.Domain.Entities;
using BankBuddy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Infrastructure.Repositories
{
    public class UserRepository(BankBuddyDbContext context) : GenericRepository<User>(context), IUserRepository
    {
        private readonly BankBuddyDbContext _context = context;

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
