using BankBuddy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankBuddy.Infrastructure.Configurations
{
    public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.HasKey(r => r.BankAccountId);

            builder.Property(r => r.BankAccountId)
                   .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(r => r.AccountNumber)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.HasIndex(b => b.AccountNumber)
                    .IsUnique();

            builder.Property(b => b.Balance)
                   .HasColumnType("decimal(18,2)");

            builder.HasOne(b => b.User)
                   .WithMany(u => u.BankAccounts)
                   .HasForeignKey(b => b.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
