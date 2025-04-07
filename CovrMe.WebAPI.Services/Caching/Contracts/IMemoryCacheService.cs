using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.Caching.Contracts
{
    public interface IMemoryCacheService
    {
        string this[string key] { get; }

        void Set(string key, string value);

        void Remove(string key);
    }
}
