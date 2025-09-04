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
    public class RoleConfiguration : BaseEntityConfiguration<Role>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Role> builder)
        {
            // Table Name
            builder.ToTable("Roles");

            // Properties
            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(50);


            // Indexes
            builder.HasIndex(u => u.Name)
                .IsUnique()
                .HasDatabaseName("IX_Roles_Name");

            // Relationship
            builder.HasMany(u => u.RolePermissions)
                .WithOne(rt => rt.Role)
                .HasForeignKey(rt => rt.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
