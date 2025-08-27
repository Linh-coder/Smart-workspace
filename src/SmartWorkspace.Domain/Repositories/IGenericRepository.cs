using SmartWorkspace.Domain.Common;
using SmartWorkspace.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Domain.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<T?> GetByIdAsync(Guid id);

        // Apply Specification
        Task<T?> GetEntityWithSpec(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);
        Task<IReadOnlyList<T>> GetListAsync(ISpecification<T> spec);

    }
}
