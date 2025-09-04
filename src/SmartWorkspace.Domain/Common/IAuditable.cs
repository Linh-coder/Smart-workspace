using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Domain.Common
{
    public interface IAuditableEntity
    {
        DateTime CreatedAt { get; }
        DateTime UpdatedAt { get; }
        string? CreatedBy { get; }
        string? UpdatedBy { get; }

        void MarkAsUpdated(string? updatedBy = null);
    }
}
