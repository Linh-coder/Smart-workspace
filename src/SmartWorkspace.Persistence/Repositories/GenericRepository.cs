using Microsoft.EntityFrameworkCore;
using SmartWorkspace.Domain.Common;
using SmartWorkspace.Domain.Repositories;
using SmartWorkspace.Domain.Specifications;
using SmartWorkspace.Persistence.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate) 
            => await _dbSet.Where(predicate).ToListAsync();

        public async Task<IReadOnlyList<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        // Apply Specification
        public async Task<T?> GetEntityWithSpec(ISpecification<T> spec) => await ApplySpecification(spec).FirstOrDefaultAsync();
        public async Task<int> CountAsync(ISpecification<T> spec) => await ApplySpecification(spec).CountAsync();
        public async Task<IReadOnlyList<T>> GetListAsync(ISpecification<T> spec) => await ApplySpecification(spec).ToListAsync();
        private IQueryable<T> ApplySpecification(ISpecification<T> spec) => SpecificationEvaluator<T>.GetQuery(_dbSet.AsQueryable(), spec);
    }
}
