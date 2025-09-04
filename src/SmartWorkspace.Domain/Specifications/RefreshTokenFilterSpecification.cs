using SmartWorkspace.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Domain.Specifications
{
    public class RefreshTokenFilterSpecification : BaseSpecification<RefreshToken>
    {
        public RefreshTokenFilterSpecification(string? token = null, Guid? userId = null, bool? onlyActive = null, bool? onlyExpired =null)
        {
            if(!string.IsNullOrEmpty(token)) Criteria = rt => rt.Token == token;
            else if(userId.HasValue)
            {
                if (onlyActive == true) Criteria = rt => rt.UserId == userId.Value && !rt.IsRevoked && rt.ExpriesAt > DateTime.UtcNow;
                else if (onlyExpired == true) Criteria = rt => rt.UserId == userId.Value && rt.ExpriesAt < DateTime.UtcNow;
                else Criteria = rt => rt.UserId == userId.Value;
            }
            AddInclude(rt => rt.User);
            AddOrderBy(rt => rt.CreatedAt);
        }
    }
}
