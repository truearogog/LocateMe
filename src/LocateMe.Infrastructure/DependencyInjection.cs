using LocateMe.Application.Abstractions.Authentication;
using LocateMe.Application.Abstractions.Cache;
using LocateMe.Application.Abstractions.Data;
using LocateMe.Application.Abstractions.Events;
using LocateMe.Application.Abstractions.Providers;
using LocateMe.Application.Abstractions.Services;
using LocateMe.Infrastructure.Authentication;
using LocateMe.Infrastructure.Cache;
using LocateMe.Infrastructure.Database;
using LocateMe.Infrastructure.Events;
using LocateMe.Infrastructure.Providers;
using LocateMe.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LocateMe.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddServices()
            .AddDatabase(configuration)
            .AddCache()
            .AddHealthChecks(configuration);
        
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IActionContext, ActionContext>();

        services.AddScoped<IJwtTokenService, JwtTokenService>();

        services.AddScoped<IEmailService, TestEmailService>();

        services.AddSingleton<InMemoryMessageQueue>();
        services.AddSingleton<IEventBus, EventBus>();
        services.AddHostedService<DomainEventProcessorJob>();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Database") ?? throw new Exception("Database connection string is null");

        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(
            options => options
                .UseNpgsql(connectionString, npgsqlOptions =>
                    npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Default))
                .UseSnakeCaseNamingConvention());

        return services;
    }

    private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("Database")!);

        return services;
    }

    private static IServiceCollection AddCache(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<IMemoryCacheService, MemoryCacheService>();
        //services.AddSingleton<IDistributedCacheService, DistributedCacheService>();
        services.AddKeyedSingleton<ICacheService, MemoryCacheService>("EmailTimeoutCache");

        return services;
    }

    public static IdentityBuilder AddStores(this IdentityBuilder builder)
    {
        builder.AddEntityFrameworkStores<ApplicationDbContext>();

        return builder;
    }
}
