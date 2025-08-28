using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartWorkspace.Domain.Common;

namespace SmartWorkspace.Domain.Users
{
    public class RolePermission : BaseEntity
    {
        public Guid RoleId { get; set; }
        public Role Role { get; set; } = default!;
        public Guid PermissionId { get; set; }
        public Permission Permission { get; set; } = default!;

    }
}
