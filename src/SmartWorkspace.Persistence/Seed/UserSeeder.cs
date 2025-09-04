using Microsoft.AspNetCore.Identity;
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
    public class UserSeeder : BaseSeeder
    {
        public override int Order => 4;

        public override async Task SeedAsync(AppDbContext context)
        {
            if (await context.Users.AnyAsync()) return;

            var passwordHasher = new PasswordHasher<User>();

            var users = new List<User>
            {
                new()
                {
                    Id = Guid.Parse("20000000-0000-0000-0000-000000000001"),
                    FullName = "Super Administrator",
                    Email = "superadmin@smartworkspace.com",
                    IsEmailConfirmed = true,
                    IsActive = true,
                    CreatedAt = Now,
                    CreatedBy = "System"
                },
                new()
                {
                    Id = Guid.Parse("20000000-0000-0000-0000-000000000002"),
                    FullName = "System Administrator",
                    Email = "admin@smartworkspace.com",
                    IsEmailConfirmed = true,
                    IsActive = true,
                    CreatedAt = Now,
                    CreatedBy = "System"
                },
                new()
                {
                    Id = Guid.Parse("20000000-0000-0000-0000-000000000003"),
                    FullName = "John Manager",
                    Email = "manager@smartworkspace.com",
                    IsEmailConfirmed = true,
                    IsActive = true,
                    CreatedAt = Now,
                    CreatedBy = "System"
                },
                new()
                {
                    Id = Guid.Parse("20000000-0000-0000-0000-000000000004"),
                    FullName = "Alice Johnson",
                    Email = "alice.johnson@smartworkspace.com",
                    IsEmailConfirmed = true,
                    IsActive = true,
                    CreatedAt = Now,
                    CreatedBy = "System"
                },
                new()
                {
                    Id = Guid.Parse("20000000-0000-0000-0000-000000000005"),
                    FullName = "Bob Wilson",
                    Email = "bob.wilson@smartworkspace.com",
                    IsEmailConfirmed = true,
                    IsActive = true,
                    CreatedAt = Now,
                    CreatedBy = "System"
                }
            };

            // Hash passwords for all users (default: "SmartWorkspace@2024")
            foreach (var user in users)
            {
                user.PasswordHash = passwordHasher.HashPassword(user, "SmartWorkspace@2024");
            }

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }
    }
}
