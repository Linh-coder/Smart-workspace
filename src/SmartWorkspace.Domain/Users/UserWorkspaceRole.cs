using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Domain.Users
{
    public class UserWorkspaceRole
    {
        public Guid UserId {  get; set; }
        public User user { get; set; } = default!;
        public Guid WorkspaceId { get; set; }
        public Workspace Workspace { get; set; } = default!;
        public Guid RoleId { get; set; }
        public Role Role { get; set; } = default!;
    }
}
