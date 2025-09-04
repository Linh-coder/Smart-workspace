using SmartWorkspace.API.Extensions;
using SmartWorkspace.Application.Common.Extensions;
using SmartWorkspace.Application.Common.Interfaces;
using SmartWorkspace.Infrastructure.Extension;
using SmartWorkspace.Infrastructure.Services;
using SmartWorkspace.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

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

app.UseHttpsRedirection();
app.UseExceptionMiddleware();

app.Run();
