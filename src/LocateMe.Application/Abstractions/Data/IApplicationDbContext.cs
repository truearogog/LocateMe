using LocateMe.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace LocateMe.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Role> Roles { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
