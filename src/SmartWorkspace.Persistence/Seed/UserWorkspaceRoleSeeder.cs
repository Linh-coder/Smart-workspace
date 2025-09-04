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
    public class UserWorkspaceRoleSeeder : BaseSeeder
    {
        public override int Order => 6;

        public override async Task SeedAsync(AppDbContext context)
        {
            if (await context.UserWorkspaceRoles.AnyAsync()) return;

            // Get Role entities từ database
            var superAdminRole = await context.Roles.FirstAsync(r => r.Name == "SuperAdmin");
            var adminRole = await context.Roles.FirstAsync(r => r.Name == "Admin");
            var managerRole = await context.Roles.FirstAsync(r => r.Name == "Manager");
            var userRole = await context.Roles.FirstAsync(r => r.Name == "User");
            var viewerRole = await context.Roles.FirstAsync(r => r.Name == "Viewer");

            var userWorkspaceRoles = new List<UserWorkspaceRole>
            {
                // SuperAdmin - Owner of Default Workspace
                new()
                {
                    Id = NewGuid(),
                    UserId = Guid.Parse("20000000-0000-0000-0000-000000000001"),
                    WorkspaceId = Guid.Parse("30000000-0000-0000-0000-000000000001"),
                    RoleId = superAdminRole.Id,     
                    IsActive = true,
                    //JoinedAt = Now,
                    //CreatedAt = Now,
                    CreatedBy = "System"
                },

                // Admin - Admin of Default Workspace  
                new()
                {
                    Id = NewGuid(),
                    UserId = Guid.Parse("20000000-0000-0000-0000-000000000002"),
                    WorkspaceId = Guid.Parse("30000000-0000-0000-0000-000000000001"),
                    RoleId = adminRole.Id,            
                    IsActive = true,
                    //JoinedAt = Now,
                    //CreatedAt = Now,
                    CreatedBy = "System"
                },

                // Manager - Manager of Development Team
                new()
                {
                    Id = NewGuid(),
                    UserId = Guid.Parse("20000000-0000-0000-0000-000000000003"),
                    WorkspaceId = Guid.Parse("30000000-0000-0000-0000-000000000002"),
                    RoleId = managerRole.Id,         
                    IsActive = true,
                    //JoinedAt = Now,
                    //CreatedAt = Now,
                    CreatedBy = "System"
                },

                // Alice - User in Development Team
                new()
                {
                    Id = NewGuid(),
                    UserId = Guid.Parse("20000000-0000-0000-0000-000000000004"),
                    WorkspaceId = Guid.Parse("30000000-0000-0000-0000-000000000002"),
                    RoleId = userRole.Id,            
                    IsActive = true,
                    //JoinedAt = Now,
                    //CreatedAt = Now,
                    CreatedBy = "System"
                },

                // Bob - User in Marketing Department
                new()
                {
                    Id = NewGuid(),
                    UserId = Guid.Parse("20000000-0000-0000-0000-000000000005"),
                    WorkspaceId = Guid.Parse("30000000-0000-0000-0000-000000000003"),
                    RoleId = userRole.Id,           
                    IsActive = true,
                    //JoinedAt = Now,
                    //CreatedAt = Now,
                    CreatedBy = "System"
                },

                // Alice - Viewer in Marketing Department (cross-team access)
                new()
                {
                    Id = NewGuid(),
                    UserId = Guid.Parse("20000000-0000-0000-0000-000000000004"),
                    WorkspaceId = Guid.Parse("30000000-0000-0000-0000-000000000003"),
                    RoleId = viewerRole.Id,            
                    IsActive = true,
                    //JoinedAt = Now,
                    //CreatedAt = Now,
                    CreatedBy = "System"
                }
            };

            await context.UserWorkspaceRoles.AddRangeAsync(userWorkspaceRoles);
            await context.SaveChangesAsync();
        }
    }
}
