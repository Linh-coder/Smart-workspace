using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartWorkspace.Domain.Common;

namespace SmartWorkspace.Domain.Entities.Users
{
    public class RolePermission : AuditableEntity
    {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }
        public Role Role { get; set; } = default!;
        public Permission Permission { get; set; } = default!;

    }
}
