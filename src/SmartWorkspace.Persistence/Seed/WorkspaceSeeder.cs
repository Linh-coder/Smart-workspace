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
    public class WorkspaceSeeder : BaseSeeder
    {
        public override int Order => 5;

        public override async Task SeedAsync(AppDbContext context)
        {
            if (await context.Workspaces.AnyAsync()) return;

            var workspaces = new List<Workspace>
            {
                new()
                {
                    Id = Guid.Parse("30000000-0000-0000-0000-000000000001"),
                    Name = "Default Workspace",
                    Description = "The default workspace for new users and general purpose collaboration",
                    IsActive = true,
                    //IsPublic = true,
                    //MaxMembers = 100,
                    //Settings = """{"allowInvites": true, "allowPublicProjects": false, "theme": "default"}""",
                    //CreatedAt = Now,
                    CreatedBy = "System"
                },
                new()
                {
                    Id = Guid.Parse("30000000-0000-0000-0000-000000000002"),
                    Name = "Development Team",
                    Description = "Workspace for software development projects and collaboration",
                    IsActive = true,
                    //IsPublic = false,
                    //MaxMembers = 50,
                    //Settings = """{"allowInvites": false, "allowPublicProjects": true, "theme": "dark"}""",
                    //CreatedAt = Now,
                    CreatedBy = "System"
                },
                new()
                {
                    Id = Guid.Parse("30000000-0000-0000-0000-000000000003"),
                    Name = "Marketing Department",
                    Description = "Marketing team workspace for campaigns and content management",
                    IsActive = true,
                    //IsPublic = false,
                    //MaxMembers = 30,
                    //Settings = """{"allowInvites": true, "allowPublicProjects": false, "theme": "light"}""",
                    //CreatedAt = Now,
                    CreatedBy = "System"
                }
            };

            await context.Workspaces.AddRangeAsync(workspaces);
            await context.SaveChangesAsync();
        }
    }
}
