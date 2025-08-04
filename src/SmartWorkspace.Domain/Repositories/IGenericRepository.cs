using SmartWorkspace.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Domain.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FinAsync(Expression<Func<T, bool>> predicate);

        Task AddAsync(T entity);
        void Update(T enity);
        void Delete(T entity);

        IQueryable<T> Query();
        Task<(IEnumerable<T> Items, int TotalCount)> GetPageAsync(
            int pageIndex,
            int pageSize,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null
            );
    }
}
