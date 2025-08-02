using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Application.Common.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(string userId, string role);
    }
}
