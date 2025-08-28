using Microsoft.EntityFrameworkCore;
using SmartWorkspace.Domain.Common;
using SmartWorkspace.Domain.Repositories;
using SmartWorkspace.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            if (_repositories.ContainsKey(typeof(T)))
                return (IGenericRepository<T>)_repositories[typeof(T)];

            var repo = new GenericRepository<T>(_context);
            _repositories.Add(typeof(T), repo);
            return repo;
        }

        public async Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in _context.ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        // entry.Property(nameof(AuditableEntity.CreatedAt)).CurrentValue = DateTime.UtcNow;
                        entry.Entity.MarkAsCreated();
                        break;
                    case EntityState.Modified:
                        // entry.Property(nameof(AuditableEntity.UpdatedAt)).CurrentValue = DateTime.UtcNow;
                        entry.Entity.MarkAsUpdated();
                        break;
                }
            }

            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
