using Microsoft.Extensions.Caching.Memory;

namespace Api.Interfaces
{
    public interface ICacheService
    {
        T GetData<T>(string key);
        
        bool SetData<T>(string key, T value);
        
        bool RemoveData(string key);
    }
}
