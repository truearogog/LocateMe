namespace LocateMe.Application.Abstractions.Cache;

public readonly record struct CacheEntryOptions(
    DateTimeOffset? AbsoluteExpiration,
    TimeSpan? AbsoluteExpirationRelativeToNow,
    TimeSpan? SlidingExpiration);
