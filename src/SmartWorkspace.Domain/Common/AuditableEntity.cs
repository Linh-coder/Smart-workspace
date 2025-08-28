using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Domain.Common
{
    public class AuditableEntity : BaseEntity, IAuditableEntity
    {
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; protected set; }
        public string? CreatedBy { get; protected set; }
        public string? UpdatedBy { get; protected set; }

        public void MarkAsCreated(string? createdBy = null)
        {
            CreatedAt = DateTime.UtcNow;
            CreatedBy = createdBy;
        }

        public void MarkAsUpdated(string? updatedBy = null)
        {
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updatedBy;
        }
    }
}
