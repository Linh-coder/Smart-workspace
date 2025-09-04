using SmartWorkspace.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Persistence.Seed.Common
{
    public abstract class BaseSeeder : ISeeder
    {
        public abstract int Order { get; }
        public abstract Task SeedAsync(AppDbContext context);
        protected static DateTime Now => DateTime.UtcNow;
        protected static Guid NewGuid() => Guid.NewGuid();
    }

    public interface ISeeder
    {
        Task SeedAsync(AppDbContext context);
        int Order { get; }
    }
}
