using SmartWorkspace.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Persistence.Seed.Common
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            var seeders = new List<ISeeder>
            {
                new PermissionSeeder(),
                new RoleSeeder(),
                new RolePermissionSeeder(),
                new UserSeeder(),
                new WorkspaceSeeder(),
                new UserWorkspaceRoleSeeder(),
            };

            // Execute seeder in order
            foreach (var seeder in seeders.OrderBy(x => x.Order))
            {
                await seeder.SeedAsync(context);
            }
        }
    }
}
