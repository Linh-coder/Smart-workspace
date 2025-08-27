using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Application.DTOs
{
    public record RegisterUserRequest( string UserName, string Email, string Password);
}
