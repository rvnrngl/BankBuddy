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
    public class TransactionConfiguration :  IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.TransactionId);

            builder.Property(t => t.TransactionId)
                   .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(t => t.Amount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(t => t.Type)
                   .IsRequired()
                   .HasConversion<string>();

            builder.Property(t => t.Status)
                   .IsRequired()
                   .HasDefaultValue(TransactionStatus.Completed)
                   .HasConversion<string>();

            builder.Property(t => t.Description)
                   .HasMaxLength(255);

            builder.Property(t => t.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(t => t.ReferenceId)
                   .HasMaxLength(100);

            builder.HasOne(t => t.FromAccount)
                   .WithMany()
                   .HasForeignKey(t => t.FromAccountId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.ToAccount)
                   .WithMany()
                   .HasForeignKey(t => t.ToAccountId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
