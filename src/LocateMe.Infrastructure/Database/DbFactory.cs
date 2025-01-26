using LocateMe.Application.Abstractions.Providers;
using LocateMe.Infrastructure.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LocateMe.Infrastructure.Database;

internal sealed class DbFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        builder
            .UseNpgsql("Host=127.0.0.1;Port=5432;Database=locateme.dev;Username=postgres;Password=postgres;Include Error Detail=true", npgsqlOptions =>
                npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Default))
            .UseSnakeCaseNamingConvention();
        return new ApplicationDbContext(builder.Options, new DateTimeProvider(), new SystemActionContext());
    }
}

internal sealed class SystemActionContext : IActionContext
{
    public Lazy<string> SourceId { get; } = new(() => "system");
}

