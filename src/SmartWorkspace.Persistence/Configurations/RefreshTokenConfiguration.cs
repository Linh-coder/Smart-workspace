using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartWorkspace.Domain.Entities;
using SmartWorkspace.Domain.Entities.Users;
using SmartWorkspace.Persistence.Configurations.BaseConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Persistence.Configurations
{
    public class RefreshTokenConfiguration : BaseAuditEntityConfiguration<RefreshToken>
    {
        protected override void ConfigurationEntity(EntityTypeBuilder<RefreshToken> builder)
        {
            // Table Name
            builder.ToTable("RefreshTokens");

            // Properties
            builder.Property(x => x.Token)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.IsRevoked)
              .HasDefaultValue(false);

            builder.Property(x => x.RevokeReason)
                .HasMaxLength(500);

            builder.Property(x => x.ReplaceByToken)
                .HasMaxLength(500);

            builder.Property(x => x.RevokeByIp)
                .HasMaxLength(500);

            builder.Property(x => x.UserId)
                .IsRequired();


            // Indexes
            
            builder.HasIndex(u => u.Token)
                .IsUnique()
                .HasDatabaseName("IX_RefreshTokens_Token");

            builder.HasIndex(u => u.UserId)
                .IsUnique()
                .HasDatabaseName("IX_RefreshTokens_UserId");

            builder.HasIndex(u => u.ExpriesAt)
                .IsUnique()
                .HasDatabaseName("IX_RefreshTokens_ExpiresAt");


            builder.HasIndex(u => new {u.UserId, u.IsRevoked, u.ExpriesAt})
                .IsUnique()
                .HasDatabaseName("IX_RefreshTokens_UserId_IsRevoke_ExpiresAt");


            // Relationship
            builder.HasOne(u => u.User)
                .WithMany(rt => rt.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
