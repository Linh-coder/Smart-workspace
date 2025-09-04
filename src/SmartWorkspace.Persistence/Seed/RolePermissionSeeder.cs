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
    public class RolePermissionSeeder : BaseSeeder
    {
        public override int Order => 3;

        public override async Task SeedAsync(AppDbContext context)
        {
            if (await context.RolePermissions.AnyAsync()) return;

            var roles = await context.Roles.ToListAsync();
            var permissions = await context.Permissions.ToListAsync();

            var rolePermissions = new List<RolePermission>();

            // SuperAdmin - All Permissions
            var superAdminRole = roles.First(x => x.Name == "SuperAdmin");
            rolePermissions.AddRange(permissions.Select(p => new RolePermission
            {
                RoleId = superAdminRole.Id,
                PermissionId = p.Id,
                CreatedBy = "System"
            }));

            // Admin - Most permissions except full system admin
            var adminRole = roles.First(r => r.Name == "Admin");
            var adminPermissions = permissions.Where(p =>
                !p.Key.StartsWith("System.Admin") &&
                !p.Key.StartsWith("System.ManageRoles")).ToList();

            rolePermissions.AddRange(adminPermissions.Select(p => new RolePermission
            {
                RoleId = adminRole.Id,
                PermissionId = p.Id,
                CreatedBy = "System"
            }));

            // Manager - Workspace and user management
            var managerRole = roles.First(r => r.Name == "Manager");
            var managerPermissions = permissions.Where(p =>
                p.Key.StartsWith("Workspace.") ||
                p.Key.StartsWith("Project.") ||
                p.Key.StartsWith("User.ViewProfile") ||
                p.Key.StartsWith("User.ViewOtherProfiles")).ToList();

            rolePermissions.AddRange(managerPermissions.Select(p => new RolePermission
            {
                RoleId = managerRole.Id,
                PermissionId = p.Id,
                CreatedBy = "System"
            }));

            // User - Basic permissions
            var userRole = roles.First(r => r.Name == "User");
            var userPermissions = permissions.Where(p =>
                p.Key.StartsWith("User.ViewProfile") ||
                p.Key.StartsWith("User.EditProfile") ||
                p.Key.StartsWith("Workspace.View") ||
                p.Key.StartsWith("Project.View")).ToList();

            rolePermissions.AddRange(userPermissions.Select(p => new RolePermission
            {
                RoleId = userRole.Id,
                PermissionId = p.Id,
                CreatedBy = "System"
            }));

            // Viewer - Read-only permissions
            var viewerRole = roles.First(r => r.Name == "Viewer");
            var viewerPermissions = permissions.Where(p =>
                p.Key.StartsWith("User.ViewProfile") ||
                p.Key.StartsWith("Workspace.View") ||
                p.Key.StartsWith("Project.View")).ToList();

            rolePermissions.AddRange(viewerPermissions.Select(p => new RolePermission
            {
                RoleId = viewerRole.Id,
                PermissionId = p.Id,
                CreatedBy = "System"
            }));

            await context.RolePermissions.AddRangeAsync(rolePermissions);
            await context.SaveChangesAsync();
        }
    }
}
