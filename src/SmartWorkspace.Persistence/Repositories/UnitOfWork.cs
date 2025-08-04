using Microsoft.EntityFrameworkCore;
using SmartWorkspace.Domain.Common;
using SmartWorkspace.Domain.Context;
using SmartWorkspace.Domain.Repositories;
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

        public async Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            foreach(var entry in _context.ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.Touch();
                }
            }

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
