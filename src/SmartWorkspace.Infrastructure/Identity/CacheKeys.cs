using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Infrastructure.Identity
{
    public static class CacheKeys
    {
        public static string UserPermission(Guid userId, Guid workspaceId) => $"perm:{userId}:{workspaceId}";
    }
}
