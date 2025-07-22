using BankBuddy.Domain.Entities;
using BankBuddy.Domain.Enums;
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
            builder.HasKey(b => b.BankAccountId);

            builder.Property(b => b.BankAccountId)
                   .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(b => b.AccountNumber)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.HasIndex(b => b.AccountNumber)
                    .IsUnique();

            builder.Property(b => b.Balance)
                   .HasColumnType("decimal(18,2)");

            builder.Property(b => b.AccountStatus)
                   .HasDefaultValue(AccountStatus.Active);

            builder.Property(u => u.CreatedAt)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(b => b.User)
                   .WithMany(u => u.BankAccounts)
                   .HasForeignKey(b => b.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
