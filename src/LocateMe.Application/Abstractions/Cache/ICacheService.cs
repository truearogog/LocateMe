namespace LocateMe.Application.Abstractions.Cache;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);
    Task<byte[]?> GetAsync(string key, CancellationToken cancellationToken = default);
    Task<string?> GetStringAsync(string key, CancellationToken cancellationToken = default);

    Task SetAsync<T>(string key, T value, CacheEntryOptions cacheEntryOptions = default,
        CancellationToken cancellationToken = default);

    Task SetAsync(string key, byte[] value, CacheEntryOptions cacheEntryOptions = default,
        CancellationToken cancellationToken = default);

    Task SetStringAsync(string key, string value, CacheEntryOptions cacheEntryOptions = default,
        CancellationToken cancellationToken = default);
}
