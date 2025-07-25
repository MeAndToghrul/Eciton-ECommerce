using Eciton.Application.Abstractions;

namespace Eciton.Persistence.Implements;
public class RateLimitService : IRateLimitService
{
    private readonly ICacheService _cache;

    public RateLimitService(ICacheService cache)
    {
        _cache = cache;
    }

    public async Task<bool> IsBlocked(string ip)
    {
        var value = await _cache.GetAsync<bool>($"BlockedIP_{ip}");
        return value;
    }

    public async Task RegisterFailedAttempt(string ip)
    {
        string failKey = $"FailedLogin_{ip}";
        string blockKey = $"BlockedIP_{ip}";

        var value = await _cache.GetAsync<int>(failKey);
        int count = value == default ? 0 : value;
        count++;

        await _cache.SetAsync(failKey, count, 1200); // saniye olaraq verdiyin üçün 10 dəq = 600

        if (count >= 15)
        {
            await _cache.SetAsync(blockKey, true, 7200); // 1 gün = 86400 saniye
            _cache.Delete(failKey);
        }
    }
}