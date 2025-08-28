using SmartWorkspace.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Domain.Specifications.UserSpecifications
{
    public class UserFilterSpecification : BaseSpecification<User>
    {
        public UserFilterSpecification(string? name = null, string? email = null) 
            : base(u =>
                (string.IsNullOrEmpty(name) || u.FullName == name) &&
                (string.IsNullOrEmpty(email) || u.Email == email)
            )
        {
            AddOrderBy(u => u.CreatedAt);
        }
    }
}
