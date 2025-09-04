using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartWorkspace.Domain.Entities.Users;
using SmartWorkspace.Persistence.Configurations.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Persistence.Configurations
{
    public class UserWorkspaceRoleConfiguration : BaseEntityConfiguration<UserWorkspaceRole>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<UserWorkspaceRole> builder)
        {
            builder.ToTable("UserWorkspaceRoles");

            // Properties
            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.WorkspaceId)
               .IsRequired();

            builder.Property(x => x.RoleId)
               .IsRequired();

            builder.Property(x => x.IsActive)
               .HasDefaultValue(true);

            // Indexes 
            builder.HasIndex(x => new { x.UserId, x.WorkspaceId })
                .IsUnique()
                .HasDatabaseName("IX_UserWorkspaceRoles_UserId_WorkspaceId");

            builder.HasIndex(x => x.WorkspaceId)
                .HasDatabaseName("IX_UserWorkspaceRoles_WorkspaceId");

            // Relationship
            builder.HasOne(x => x.User)
                .WithMany(x => x.WorkspaceRoles)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Workspace)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.WorkspaceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Role)
                .WithMany(x => x.UserWorkspaceRoles)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
