using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartWorkspace.Domain.Entities;
using SmartWorkspace.Domain.Entities.Users;
using SmartWorkspace.Domain.Users.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<UserWorkspaceRole> UserWorkspaceRoles { get; set; }
        public DbSet<Workspace> Workspaces { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            ConfigureGlobalSettings(modelBuilder);
        }

        private void ConfigureGlobalSettings(ModelBuilder modelBuilder)
        {
           var allProperties = modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties())
                .ToList();

            var propertyGroups = allProperties
                .GroupBy(p => p.ClrType)
                .ToDictionary(g => g.Key, g => g.ToList());

            if (propertyGroups.ContainsKey(typeof(decimal)) || propertyGroups.ContainsKey(typeof(decimal?))) 
                propertyGroups[typeof(decimal)].ForEach(p => p.SetColumnType("decimal(18,2)"));
        }
    }
}
