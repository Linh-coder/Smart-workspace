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
    public class PermissionConfiguration : BaseEntityConfiguration<Permission>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Permission> builder)
        {
            // Table Name
            builder.ToTable("Permissions");

            // Properties
            builder.Property(u => u.Key)
                .IsRequired()
                .HasMaxLength(50);


            // Indexes
            builder.HasIndex(u => u.Key)
                .IsUnique()
                .HasDatabaseName("IX_Permission_Key");

            // Relationship
            builder.HasMany(u => u.RolePermissions)
                .WithOne(rt => rt.Permission)
                .HasForeignKey(rt => rt.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
