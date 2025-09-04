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
    public class PermissionSeeder : BaseSeeder
    {
        public override int Order => 1;

        public override async Task SeedAsync(AppDbContext context)
        {
            if(await context.Permissions.AnyAsync()) return;

            var permissions = new List<Permission>
            {
                // System Permissions
                new()
                {
                    Id = Guid.Parse("00000001-0001-0001-0001-000000000001"),
                    Key = "System.Admin",
                    Description = "Full system administration access"
                },
                new()
                {
                    Id = Guid.Parse("00000001-0001-0001-0001-000000000002"),
                    Key = "System.ViewLogs",
                    Description = "View system logs and audit trails"
                },
                new()
                {
                    Id = Guid.Parse("00000001-0001-0001-0001-000000000003"),
                    Key = "System.ManageUsers",
                    Description = "Manage user accounts"
                },
                new()
                {
                    Id = Guid.Parse("00000001-0001-0001-0001-000000000004"),
                    Key = "System.ManageRoles",
                    Description = "Manage system roles and permissions"
                },

                // Workspace Permissions
                new()
                {
                    Id = Guid.Parse("00000002-0002-0002-0002-000000000001"),
                    Key = "Workspace.Create",
                    Description = "Create new workspaces"
                },
                new()
                {
                    Id = Guid.Parse("00000002-0002-0002-0002-000000000002"),
                    Key = "Workspace.View",
                    Description = "View workspace details"
                },
                new()
                {
                    Id = Guid.Parse("00000002-0002-0002-0002-000000000003"),
                    Key = "Workspace.Edit",
                    Description = "Edit workspace settings and properties"
                },
                new()
                {
                    Id = Guid.Parse("00000002-0002-0002-0002-000000000004"),
                    Key = "Workspace.Delete",
                    Description = "Delete workspaces"
                },
                new()
                {
                    Id = Guid.Parse("00000002-0002-0002-0002-000000000005"),
                    Key = "Workspace.ManageMembers",
                    Description = "Add, remove, and manage workspace members"
                },

                // User Permissions
                new()
                {
                    Id = Guid.Parse("00000003-0003-0003-0003-000000000001"),
                    Key = "User.ViewProfile",
                    Description = "View user profile information"
                },
                new()
                {
                    Id = Guid.Parse("00000003-0003-0003-0003-000000000002"),
                    Key = "User.EditProfile",
                    Description = "Edit own profile information"
                },
                new()
                {
                    Id = Guid.Parse("00000003-0003-0003-0003-000000000003"),
                    Key = "User.ViewOtherProfiles",
                    Description = "View other users' profiles"
                }
            };

            await context.Permissions.AddRangeAsync(permissions);
            await context.SaveChangesAsync();
        }
    }
}
