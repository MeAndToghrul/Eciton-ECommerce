using Eciton.Application.Abstractions;
using Microsoft.Extensions.Caching.Memory;

namespace Eciton.Persistence.Implements;
public class LocalCacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    public LocalCacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        T? value = default(T);
        await Task.Run(() =>
        {
            _cache.TryGetValue<T>(key, out value);
        });
        return value;
    }

    public async Task SetAsync<T>(string key, T data, int seconds = 300)
    {
        await Task.Run(() =>
        {
            _cache.Set<T>(key, data, DateTime.Now.AddSeconds(seconds));
        });
    }

    public async Task<bool> IsExistsAsync(string key)
    {
        var data = await Task.Run(() => _cache.Get(key));
        if (data == null)
            return false;

        return true;
    }

    public void Delete(string key)
        => _cache.Remove(key);
}
