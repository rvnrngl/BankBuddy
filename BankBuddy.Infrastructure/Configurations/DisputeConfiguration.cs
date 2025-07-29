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
    public class DisputeConfiguration : IEntityTypeConfiguration<Dispute>
    {
        public void Configure(EntityTypeBuilder<Dispute> builder)
        {
            builder.HasKey(d => d.DisputeId);

            builder.Property(t => t.DisputeId)
                   .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(d => d.Reason)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(d => d.Status)
                   .IsRequired()
                   .HasDefaultValue(DisputeStatus.UnderReview)
                   .HasConversion<string>();

            builder.Property(d => d.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(d => d.ResolvedAt)
                   .IsRequired(false);

            builder.HasOne(d => d.Transaction)
                   .WithMany()
                   .HasForeignKey(d => d.TransactionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
