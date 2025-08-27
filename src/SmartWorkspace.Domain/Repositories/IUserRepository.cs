using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartWorkspace.Domain.Users;

namespace SmartWorkspace.Domain.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByUserNameAsync(string userName);
        Task<User?> GetByIdAsync(Guid id);
    }
}
