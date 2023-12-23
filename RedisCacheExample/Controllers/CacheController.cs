using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace RedisCacheExample.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CacheController : Controller
{
    private readonly IDistributedCache _distributedCache;
    private readonly ICacheManager _cacheManager;
    private string cacheName = "best-player";

    public CacheController(IDistributedCache distributedCache, ICacheManager cacheManager)
    {
        _distributedCache = distributedCache;
        _cacheManager = cacheManager;
    }

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        string? value = await _distributedCache.GetStringAsync(cacheName);

        if (!string.IsNullOrEmpty(value))
        {
            return Ok(new { Result = value, From = "cache" });
        }

        return Ok(new { Result = "no cache", From = "(!)" });
    }

    [HttpPost]
    public async Task<ActionResult> Set([FromBody] string value)
    {
        await _distributedCache.SetStringAsync(cacheName, value);

        return Ok(new { Result = "cache created!" });
    }
    
    [HttpDelete]
    public async Task<ActionResult> Delete()
    {
        await _distributedCache.RemoveAsync(cacheName);

        return Ok(new { Result = "cache deleted!" });
    }
    
    [HttpGet("customized")]
    public async Task<ActionResult> CustomizedGet()
    {
        string? data = await _cacheManager.Get(cacheName);

        if (!string.IsNullOrEmpty(data))
        {
            return Ok(new { Result = data, From = "cache" });
        }
        
        return Ok(new { Result = "no cache", From = "(!)" });
    }
    
    [HttpPost("customized")]
    public async Task<ActionResult> CustomizedSet([FromBody] string value)
    {
        await _cacheManager.AddWithTimeSpan(cacheName, value, TimeSpan.FromSeconds(50));
        return Ok(new { Result = "cache created!" });
    }
    
    [HttpDelete("customized")]
    public async Task<ActionResult> CustomizedDelete()
    {
        await _cacheManager.Remove(cacheName);

        return Ok(new { Result = "cache deleted!" });
    }
}