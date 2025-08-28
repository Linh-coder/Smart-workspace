using SmartWorkspace.Domain.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Domain.Entities.Users
{
    public class Workspace : AuditableEntity
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public bool IsActive { get; set; } = true;
        public Guid OwnerId { get; set; }
        public ICollection<UserWorkspaceRole> Members { get; set; } = new List<UserWorkspaceRole>();

    }
}
