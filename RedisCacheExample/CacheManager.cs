using Microsoft.Extensions.Caching.Distributed;

namespace RedisCacheExample;

public class CacheManager : ICacheManager
{
    private readonly IDistributedCache _distributedCache;
    private string cacheBase = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development ? "DEV" : "PROD";

    public CacheManager(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task Add(string key, string value, TimeSpan? expireTime)
    {
        await _distributedCache.SetStringAsync($"{cacheBase}_{key}", value, new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)));
    }

    public async Task<string?> Get(string key)
    {
        return await _distributedCache.GetStringAsync($"{cacheBase}_{key}");
    }

    public async Task Remove(string key)
    {
        await _distributedCache.RemoveAsync($"{cacheBase}_{key}");
    }
}