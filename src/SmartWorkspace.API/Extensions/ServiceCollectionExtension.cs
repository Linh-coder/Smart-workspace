using SmartWorkspace.Application.Common.Interfaces;
using SmartWorkspace.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Domain.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddWebAPIService(this IServiceCollection services)
        {
            services.AddScoped<IPermissionService, PermissionService>()
                    .AddMemoryCache();

            return services;
        }
    }
}
