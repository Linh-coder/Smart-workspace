using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SmartWorkspace.Application.DTOs;
using SmartWorkspace.Domain.Users.Behaviors;

namespace SmartWorkspace.Application.Common.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Add MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtension).Assembly));
            services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtension).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // Add AutoMapper
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            return services;
        }
    }
}
