using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SmartWorkspace.Application.Common.Interfaces;
using SmartWorkspace.Infrastructure.Identity;
using SmartWorkspace.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Infrastructure.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IMemoryCache _cache;
        private readonly AppDbContext _db;
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(15);

        public PermissionService(AppDbContext db, IMemoryCache cache)
        {
            _db = db;
            _cache = cache;
        }

        public async Task<bool> HasPermissionAsync(Guid userId, Guid workspaceId, string permission, CancellationToken ct)
        {
            var permissions = await GetUserPermissionAsync(userId, workspaceId, ct);
            return permissions.Contains(permission);
        }

        private async Task<HashSet<string>> GetUserPermissionAsync(Guid userId, Guid workspaceId, CancellationToken cancellationToken)
        {
            var cacheKey = CacheKeys.UserPermission(userId, workspaceId);
            if (_cache.TryGetValue<HashSet<string>>(cacheKey, out var cachedPermissions))
                return cachedPermissions;

            // Load from DB
            var permissions = await _db.UserWorkspaceRoles
                .Where(x => x.UserId == userId)
                .SelectMany(x => x.Role.Permissions)
                .Select(x => x.Role.Name)
                .Distinct()
                .ToListAsync();

            var permissionSet = new HashSet<string>(permissions);
            _cache.Set(cacheKey, permissionSet, CacheDuration);
            return permissionSet;
        }
    }
}
