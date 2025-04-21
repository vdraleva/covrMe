using CovrMe.Services.Contracts;

namespace CovrMe.Services.Implementation
{
    public class CacheService : ICacheService
    {
        private string _apiUrl;
        public CacheService(string baseUrl)
        {
            this._apiUrl = baseUrl;
        }

    }
}
