using SmartWorkspace.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Domain.Entities.Users
{
    public class Permission : BaseEntity
    {
        public string Key { get; set; } = default!; // ex: "Workspace.Create"
        public string Description { get; set; } = default!;

        // Navigation properties
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
