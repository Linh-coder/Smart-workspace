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
    public class WorkspaceConfiguration : BaseAuditEntityConfiguration<Workspace>
    {
        protected override void ConfigurationEntity(EntityTypeBuilder<Workspace> builder)
        {
            // Table Name
            builder.ToTable("Workspaces");

            // Properties
            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Description)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.IsActive)
                .HasDefaultValue(true);

            // Indexes
            builder.HasIndex(u => u.Name)
                .IsUnique()
                .HasDatabaseName("IX_Workspace_Name");

            builder.HasIndex(u => u.Name)
                .IsUnique()
                .HasDatabaseName("IX_Workspace_IsActive");

            // Relationship
            builder.HasMany(u => u.UserRoles)
                .WithOne(rt => rt.Workspace)
                .HasForeignKey(rt => rt.WorkspaceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
