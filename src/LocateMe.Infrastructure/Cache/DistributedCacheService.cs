using LocateMe.Application.Abstractions.Cache;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace LocateMe.Infrastructure.Cache;

internal sealed class DistributedCacheService(IDistributedCache cache) : IDistributedCacheService
{
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        string? @string = await cache.GetStringAsync(key, cancellationToken);
        if (@string == null)
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(@string);
    }

    public async Task<byte[]?> GetAsync(string key, CancellationToken cancellationToken = default)
    {
        return await cache.GetAsync(key, cancellationToken);
    }

    public async Task<string?> GetStringAsync(string key, CancellationToken cancellationToken = default)
    {
        return await cache.GetStringAsync(key, cancellationToken);
    }

    public async Task SetAsync<T>(string key, T value, CacheEntryOptions cacheEntryOptions = default,
        CancellationToken cancellationToken = default)
    {
        string @string = JsonSerializer.Serialize(value);
        await cache.SetStringAsync(key, @string, cacheEntryOptions.ToDistributedCacheEntryOptions(), cancellationToken);
    }

    public async Task SetAsync(string key, byte[] value, CacheEntryOptions cacheEntryOptions = default,
        CancellationToken cancellationToken = default)
    {
        await cache.SetAsync(key, value, cacheEntryOptions.ToDistributedCacheEntryOptions(), cancellationToken);
    }

    public async Task SetStringAsync(string key, string value, CacheEntryOptions cacheEntryOptions = default,
        CancellationToken cancellationToken = default)
    {
        await cache.SetStringAsync(key, value, cacheEntryOptions.ToDistributedCacheEntryOptions(), cancellationToken);
    }
}
