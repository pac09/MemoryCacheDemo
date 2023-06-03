using Api.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Api.Services;
public class CacheService : ICacheService
{
    private IMemoryCache _cache;

    private MemoryCacheEntryOptions _cacheEntryOptions;

    public CacheService()
    {
    }
    
    public CacheService(IMemoryCache cache, int slidingExpiration, int absoluteExpiration)
    {
        _cache = cache;
        _cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(slidingExpiration))
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(absoluteExpiration))
            .SetPriority(CacheItemPriority.Normal);
        //.SetSize(1024);
        //I wouldn't use this but talk to Krasen if he agrees to do not limit the cache size
    }

    public T GetData<T>(string key)
    {
        try
        {
            _cache.TryGetValue(key, out T cachedItem);

            return cachedItem;
        }
        catch (Exception e)
        {
            //Log error
            throw;
        }
    }

    public bool SetData<T>(string key, T value)
    {
        try
        {
            if (!string.IsNullOrEmpty(key))
            {
                _cache.Set(key, value, _cacheEntryOptions);
            }

            return true;
        }
        catch (Exception e)
        {
            //Log error
            return false;
        }
    }
    
    public bool RemoveData(string key)
    {
        try
        {
            if (!string.IsNullOrEmpty(key))
                _cache.Remove(key);

            return true;
        }
        catch (Exception e)
        {
            //Log error
            throw;
        }

        return false;
    }
    
}