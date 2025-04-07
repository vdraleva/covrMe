using CovrMe.WebAPI.Services.Caching.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.Caching.Implementations
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _cacheOptions;

        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;

            _cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromHours(11))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(11))
                    .SetPriority(CacheItemPriority.Normal)
                    .SetSize(1024);
        }

        public string this[string key]
        {
            get
            {
                string value = string.Empty;

                if (this._cache != null)
                {
                    string result = string.Empty;
                    bool isGet = _cache.TryGetValue(key, out result);
                    value = result;
                }

                return value;
            }
        }

        public void Set(string key, string value)
        {
            string result = string.Empty;
            bool contains = _cache.TryGetValue(key, out result);         
            
            if (contains)
            {
                _cache.Remove(key);
            }

            this._cache.Set(key, value, _cacheOptions);
        }

        public void Remove(string key)
        {
            string result = string.Empty;
            bool contains = _cache.TryGetValue(key, out result);

            if (contains)
            {
                _cache.Remove(key);
            }
        }
    }
}
