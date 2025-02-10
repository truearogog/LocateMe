using LocateMe.Application.Abstractions.Cache;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace LocateMe.Infrastructure.Cache;

internal static class CacheEntryOptionsExtensions
{
    public static MemoryCacheEntryOptions ToMemoryCacheEntryOptions(this CacheEntryOptions cacheOptions)
    {
        return new MemoryCacheEntryOptions
        {
            AbsoluteExpiration = cacheOptions.AbsoluteExpiration,
            AbsoluteExpirationRelativeToNow = cacheOptions.AbsoluteExpirationRelativeToNow,
            SlidingExpiration = cacheOptions.SlidingExpiration
        };
    }

    public static DistributedCacheEntryOptions ToDistributedCacheEntryOptions(this CacheEntryOptions cacheOptions)
    {
        return new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = cacheOptions.AbsoluteExpiration,
            AbsoluteExpirationRelativeToNow = cacheOptions.AbsoluteExpirationRelativeToNow,
            SlidingExpiration = cacheOptions.SlidingExpiration
        };
    }
}
