using SmartWorkspace.Domain.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Domain.Users
{
    public class Workspace : BaseEntity
    {
        public string Name { get; set; } = default!;
        public ICollection<UserWorkspaceRole> Members { get; set; } = new List<UserWorkspaceRole>();

    }
}
