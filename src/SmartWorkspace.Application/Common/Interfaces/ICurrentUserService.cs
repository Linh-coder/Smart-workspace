using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Domain.Users.Interfaces
{
    public class ICurrentUserService
    {
        string? UserId { get; }
        bool IsAuthenticated { get; }
    }
}
