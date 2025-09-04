using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartWorkspace.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Persistence.Seed.Common
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DatabaseInitializer> _logger;

        public DatabaseInitializer(AppDbContext context, ILogger<DatabaseInitializer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            try
            {
                _logger.LogInformation("Cheking database connection...");
                if (!await _context.Database.CanConnectAsync())
                {
                    _logger.LogError("Cannot connect to database");
                    return;
                }

                // Apply pending migrations
                var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                {
                    _logger.LogInformation($"Applying {pendingMigrations.Count()} pending migrations...");
                    await _context.Database.MigrateAsync();
                    _logger.LogInformation($"Migrations applied successfuly!");
                }
                else
                {
                    _logger.LogInformation("No pending migrations found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Database migration failed.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                _logger.LogInformation("Seeding database...");
                await DatabaseSeeder.SeedAsync(_context);
                _logger.LogInformation($"Database seeded successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database seeding failed");
                throw;
            }
        }
    }

    public interface IDatabaseInitializer
    {
        Task InitializeAsync();
        Task SeedAsync();
    }
}
