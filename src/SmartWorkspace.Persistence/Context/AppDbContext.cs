using Microsoft.EntityFrameworkCore;
using SmartWorkspace.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() { }
        public DbSet<User> Users { get; set; }
    }
}
