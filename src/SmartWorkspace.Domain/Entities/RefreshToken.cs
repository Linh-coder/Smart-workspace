using SmartWorkspace.Domain.Common;
using SmartWorkspace.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Domain.Entities
{
    public class RefreshToken : AuditableEntity
    {
        public string Token { get; set; } = default!;
        public DateTime ExpriesAt { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime? RevokeAt { get; set; }
        public string? RevokeReason { get; set; }
        public Guid UserId { get; set; }
        public string? ReplaceByToken { get; set; }
        public string? RevokeByIp { get; set; }

        public virtual User User { get; set; } = null;
        public bool IsExpired => DateTime.UtcNow >= ExpriesAt;
        public bool IsActive => !IsRevoked && !IsExpired;
    }
}
