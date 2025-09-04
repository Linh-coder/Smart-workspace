using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartWorkspace.Domain.Repositories;
using SmartWorkspace.Persistence.Context;
using SmartWorkspace.Persistence.Repositories;
using SmartWorkspace.Persistence.Seed.Common;

namespace SmartWorkspace.Persistence.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration) 
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(
                connectionString, 
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
            ));

            // Database services
            services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();

            // Repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
