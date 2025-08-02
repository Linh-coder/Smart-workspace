using SmartWorkspace.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Infrastructure.Auth
{
    public class JwtTokenService : IJwtTokenService
    {
        public string GenerateToken(string userId, string role)
        {
            // TODO: Implement JWT token generation logic here.
            return "dummy-token";
        }
    }
}
