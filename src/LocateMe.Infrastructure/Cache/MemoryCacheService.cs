using LocateMe.Application.Abstractions.Cache;
using Microsoft.Extensions.Caching.Memory;

namespace LocateMe.Infrastructure.Cache;

internal sealed class MemoryCacheService(IMemoryCache cache) : IMemoryCacheService
{
    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(cache.Get<T?>(key));
    }

    public Task<byte[]?> GetAsync(string key, CancellationToken cancellationToken = default)
    {
        return GetAsync<byte[]?>(key, cancellationToken);
    }

    public Task<string?> GetStringAsync(string key, CancellationToken cancellationToken = default)
    {
        return GetAsync<string>(key, cancellationToken);
    }

    public Task SetAsync<T>(string key, T value, CacheEntryOptions cacheEntryOptions = default,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        cache.Set(key, value, cacheEntryOptions.ToMemoryCacheEntryOptions());
        return Task.CompletedTask;
    }

    public Task SetAsync(string key, byte[] value, CacheEntryOptions cacheEntryOptions = default,
        CancellationToken cancellationToken = default)
    {
        return SetAsync<byte[]>(key, value, cacheEntryOptions, cancellationToken);
    }

    public Task SetStringAsync(string key, string value, CacheEntryOptions cacheEntryOptions = default,
        CancellationToken cancellationToken = default)
    {
        return SetAsync(key, value, cacheEntryOptions, cancellationToken);
    }
}
