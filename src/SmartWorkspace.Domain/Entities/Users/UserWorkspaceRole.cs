using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartWorkspace.Domain.Common;

namespace SmartWorkspace.Domain.Entities.Users
{
    public class UserWorkspaceRole : BaseEntity
    {
        public Guid UserId {  get; set; }
        public User User { get; set; } = default!;
        public Guid WorkspaceId { get; set; }
        public Workspace Workspace { get; set; } = default!;
        public Guid RoleId { get; set; }
        public Role Role { get; set; } = default!;
    }
}
