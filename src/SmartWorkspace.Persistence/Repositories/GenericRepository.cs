using Microsoft.EntityFrameworkCore;
using SmartWorkspace.Domain.Common;
using SmartWorkspace.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> FinAsync(Expression<Func<T, bool>> predicate) 
            => await _dbSet.Where(predicate).ToListAsync();

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);


        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public IQueryable<T> Query() => _dbSet.AsQueryable();

        public async Task<(IEnumerable<T> Items, int TotalCount)> GetPageAsync(int pageIndex, int pageSize, Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            var query = _dbSet.AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            var totalCount = await query.CountAsync();

            if (orderBy != null)
                query = orderBy(query);

            var items = await query.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();

            return (items, totalCount);
        }
    }
}
