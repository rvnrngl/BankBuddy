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
    public class VerificationTokenConfiguration : IEntityTypeConfiguration<VerificationToken>
    {
        public void Configure(EntityTypeBuilder<VerificationToken> builder)
        {
            builder.HasKey(b => b.VerificationTokenId);

            builder.Property(b => b.VerificationTokenId)
                   .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(v => v.Token)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(v => v.ExpiresAt)
                   .IsRequired();

            builder.HasOne(v => v.User)
                   .WithMany(u => u.VerificationTokens)
                   .HasForeignKey(v => v.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
