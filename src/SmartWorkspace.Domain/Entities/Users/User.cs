using SmartWorkspace.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Domain.Entities.Users
{
    public class User : AuditableEntity
    {
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public bool IsEmailConfirmed { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public DateTime? LastLoginAt { get; set; }
        public ICollection<UserWorkspaceRole> WorkspaceRoles { get; set; } = new List<UserWorkspaceRole>();
    }
}
