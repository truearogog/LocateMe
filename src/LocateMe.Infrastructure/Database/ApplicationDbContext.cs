using LocateMe.Application.Abstractions.Data;
using LocateMe.Application.Abstractions.Providers;
using LocateMe.Core;
using LocateMe.Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LocateMe.Infrastructure.Database;

internal sealed class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IDateTimeProvider dateTimeProvider,
    IActionContext actionContext)
    : IdentityDbContext<User, Role, Guid>(options), IApplicationDbContext
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        builder.HasDefaultSchema(Schemas.Default);

        base.OnModelCreating(builder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<EntityEntry> entries = ChangeTracker
            .Entries()
            .Where(entry => entry.Entity is IEntity && entry.State is EntityState.Added or EntityState.Modified);

        foreach (EntityEntry entry in entries)
        {
            if (entry.State is EntityState.Added)
            {
                ((IEntity)entry.Entity).Created = dateTimeProvider.UtcNow;
                ((IEntity)entry.Entity).CreatedBy = actionContext.SourceId.Value;
            }
            ((IEntity)entry.Entity).Updated = dateTimeProvider.UtcNow;
            ((IEntity)entry.Entity).UpdatedBy = actionContext.SourceId.Value;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
