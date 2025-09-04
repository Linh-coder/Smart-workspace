using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartWorkspace.Domain.Common;
using SmartWorkspace.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Persistence.Configurations.CommonConfiguration
{
    public abstract class BaseAuditEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : AuditableEntity
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            // Primary Key
            builder.HasKey(u => u.Id);

            // Properties
            builder.Property(u => u.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            // Audit fields
            builder.Property(e => e.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.Property(e => e.UpdatedAt)
                .HasColumnType("datetime2");

            builder.Property(e => e.CreatedBy)
                .HasMaxLength(450);


            builder.Property(e => e.UpdatedBy)
                .HasMaxLength(450);

            // Add indexes for audit fields
            builder.HasIndex(e => e.CreatedAt)
                .HasDatabaseName($"IX_{typeof(T).Name}_CreatedAt");

            // Call derived configuration
            ConfigurationEntity(builder);

        }

        protected abstract void ConfigurationEntity(EntityTypeBuilder<T> builder);
    }
}
