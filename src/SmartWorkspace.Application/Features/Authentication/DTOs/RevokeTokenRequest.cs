using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Application.Features.Authentication.DTOs
{
    public class RevokeTokenRequest
    {
        [Required]
        public string RefreshToken { get; init; } = string.Empty;
        public string Reason { get; init; }
    }
}
