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
    public class RolePermissionConfiguration : BaseEntityConfiguration<RolePermission>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<RolePermission> builder)
        {
            // Table Name
            builder.ToTable("RolePermissions");

            // Properties
            builder.Property(u => u.RoleId)
                .IsRequired();

            builder.Property(u => u.PermissionId)
                .IsRequired();

            // Indexes
            builder.HasIndex(u => new {u.RoleId, u.PermissionId})
                .IsUnique()
                .HasDatabaseName("IX_RolePermission_RoleId_PermissionId");

            // Relationship
            builder.HasOne(u => u.Role)
                .WithMany(rt => rt.RolePermissions)
                .HasForeignKey(rt => rt.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.Permission)
                .WithMany(rt => rt.RolePermissions)
                .HasForeignKey(rt => rt.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
