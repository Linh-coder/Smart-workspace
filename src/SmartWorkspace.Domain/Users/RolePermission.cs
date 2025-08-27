﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Domain.Users
{
    public class RolePermission
    {
        public Guid RoleId { get; set; }
        public Role Role { get; set; } = default!;
        public Guid PermissionId { get; set; }
        public Permission Permission { get; set; } = default!;

    }
}
