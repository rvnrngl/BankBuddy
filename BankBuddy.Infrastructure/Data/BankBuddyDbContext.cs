using BankBuddy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Infrastructure.Data
{
    public class BankBuddyDbContext(DbContextOptions<BankBuddyDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BankBuddyDbContext).Assembly);

            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = Guid.NewGuid(), Name = "Customer" },
                new Role { RoleId = Guid.NewGuid(), Name = "Admin" }
            );
        }
    }
}
