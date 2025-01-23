using Microsoft.AspNetCore.Authorization;
using MultiTenantAspirization.AllowAuthorized;
using MultiTenantAspirization.AllowAuthorized.Policies.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();
builder.Services.AddControllers();

builder.Services.AddAuthorization();

builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

builder.AddNpgsqlDbContext<AuthDbContext>(connectionName: "authdb");


// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.UseHttpsRedirection();


app.Run();