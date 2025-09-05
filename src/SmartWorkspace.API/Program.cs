using Serilog;
using SmartWorkspace.API.Extensions;
using SmartWorkspace.API.Middlewares;
using SmartWorkspace.Application.Common.Extensions;
using SmartWorkspace.Application.Common.Interfaces;
using SmartWorkspace.Infrastructure.Extension;
using SmartWorkspace.Infrastructure.Services;
using SmartWorkspace.Persistence.Extensions;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddSerilLogging();
    Log.Information("Starting SmartworkSpace API");

    // Add services to the container.
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    builder.Services
        .AddWebAPIService(builder.Configuration)
        .AddApplication()
        .AddInfrastructureServices(builder.Configuration)
        .AddPersistenceServices(builder.Configuration);

    var app = builder.Build();

    //Initialize database
    await app.InitializeDatabaseAsync();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.MapOpenApi();
    }

    //app.UseExceptionMiddleware();
    app.UseHttpsRedirection();
    app.UseSerilogMiddleware();
    app.UseAuthentication();
    app.UseAuthorization();
    //app.UseMiddleware<PermissionMiddleware>();

    Log.Information("SmartworkSpace API started successfully");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "SmartWorkSpace API terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
