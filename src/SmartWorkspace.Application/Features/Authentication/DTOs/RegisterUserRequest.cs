using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Application.Features.Authentication.DTOs
{
    public record RegisterUserRequest
    {
        [Required]
        [MinLength(2)]
        public string UserName { get; init; } = default!;

        [Required]
        [EmailAddress]
        public string Email { get; init; } = default!;

        [Required]
        [MinLength(6)]
        public string Password { get; init; } = default!;

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; init; } = default!;
    }
}
