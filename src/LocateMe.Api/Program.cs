using LocateMe.Api;
using LocateMe.Api.Extensions;
using LocateMe.Domain.Users;
using LocateMe.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
builder.Services.AddAuthorization();
builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme);

builder.Services
    .AddPresentation()
    .AddInfrastructure(builder.Configuration, identityBuilder => identityBuilder.AddApiEndpoints());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.MapIdentityApi<User>();

await app.RunAsync();
