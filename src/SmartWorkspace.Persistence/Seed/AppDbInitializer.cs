using SmartWorkspace.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Domain.Seed
{
    public static class AppDbInitializer
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (!context.Users.Any())
            {
                // Seed demo data
                await context.SaveChangesAsync();
            }
        }
    }
}
