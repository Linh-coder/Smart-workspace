using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartWorkspace.Domain.Common;

namespace SmartWorkspace.Domain.Entities.Users
{
    public class UserWorkspaceRole : AuditableEntity
    {
        public Guid UserId {  get; set; }
        public Guid WorkspaceId { get; set; }
        public Guid RoleId { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public virtual Role Role { get; set; } = default!;
        public virtual User User { get; set; } = default!;
        public virtual Workspace Workspace { get; set; } = default!;
    }
}
