using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartWorkspace.Domain.Entities.Users;
using SmartWorkspace.Persistence.Configurations.CommonConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Persistence.Configurations
{
    public class UserConfiguration : BaseAuditEntityConfiguration<User>
    {
        protected override void ConfigurationEntity(EntityTypeBuilder<User> builder)
        {
            // Table Name
            builder.ToTable("Users");

            // Properties
            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(u => u.IsEmailConfirmed)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(u => u.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(u => u.LastLoginAt)
                .HasColumnType("datetime2");

            // Indexes
            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("IX_Users_Email");

            builder.HasIndex(u => u.IsActive)
                .IsUnique()
                .HasDatabaseName("IX_Users_IsActive");

            builder.HasIndex(u => new {u.Email, u.IsActive})
                .IsUnique()
                .HasDatabaseName("IX_Users_Email_IsActive");

            // Relationship
            builder.HasMany(u => u.RefreshTokens)
                .WithOne(rt => rt.User)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_RefreshToken_Users");

            builder.HasMany(u => u.WorkspaceRoles)
                .WithOne(rt => rt.User)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_UserWorkspaceRoles_Users");
        }
    }
}
