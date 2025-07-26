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

        public DbSet<BankAccount> BankAccounts { get; set; }

        public DbSet<VerificationToken> VerificationTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BankBuddyDbContext).Assembly);

            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = SeedData.CustomerRoleId, Name = "Customer" },
                new Role { RoleId = SeedData.AdminRoleId, Name = "Admin" }
            );
        }
    }
}
