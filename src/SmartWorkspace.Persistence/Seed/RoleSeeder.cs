using Microsoft.EntityFrameworkCore;
using SmartWorkspace.Domain.Entities.Users;
using SmartWorkspace.Persistence.Context;
using SmartWorkspace.Persistence.Seed.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Persistence.Seed
{
    public class RoleSeeder : BaseSeeder
    {
        public override int Order => 2;

        public override async Task SeedAsync(AppDbContext context)
        {
            if (await context.Roles.AnyAsync()) return;

            var roles = new List<Role>
            {
                new()
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                    Name = "SuperAdmin",
                    Description = "Super administrator with full system access",
                    IsActive = true,
                    IsSystemRole = true
                },
                new()
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000002"),
                    Name = "Admin",
                    Description = "System administrator with elevated privileges",
                    IsActive = true,
                    IsSystemRole = true
                },
                new()
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000003"),
                    Name = "Manager",
                    Description = "Manager role with workspace management capabilities",
                    IsActive = true,
                    IsSystemRole = false
                },
                new()
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000004"),
                    Name = "User",
                    Description = "Standard user with basic access rights",
                    IsActive = true,
                    IsSystemRole = true
                },
                new()
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000005"),
                    Name = "Viewer",
                    Description = "Read-only user with view access rights",
                    IsActive = true,
                    IsSystemRole = true
                }
            };

            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }
    }
}
