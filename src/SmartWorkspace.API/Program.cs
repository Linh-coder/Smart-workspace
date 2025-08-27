using SmartWorkspace.API.Extensions;
using SmartWorkspace.Application.Common.Extensions;
using SmartWorkspace.Application.Common.Interfaces;
using SmartWorkspace.Domain.Extensions;
using SmartWorkspace.Infrastructure.Extension;
using SmartWorkspace.Infrastructure.Services;
using SmartWorkspace.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services
    .AddWebAPIService()
    .AddApplication()
    .AddInfrastructureServices(builder.Configuration)
    .AddPersistenceServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseExceptionMiddleware();

app.Run();
