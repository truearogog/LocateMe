using LocateMe.Application.Abstractions.Providers;

namespace LocateMe.Infrastructure.Providers;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
}
