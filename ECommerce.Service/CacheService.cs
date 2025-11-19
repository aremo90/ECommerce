using ECommerce.Domin.Contract;
using ECommerce.ServiceAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Service
{
    public class CacheService : ICacheService
    {
        private readonly ICacheRepository _cacheRepository;

        public CacheService(ICacheRepository cacheRepository)
        {
            _cacheRepository = cacheRepository;
        }

        public async Task<string?> GetAsync(string cacheKey)
        {
            return await _cacheRepository.GetAsync(cacheKey);
        }

        public async Task SetAsync(string cacheKey, object value, TimeSpan timeToLive)
        {
            var CacheValue = JsonSerializer.Serialize(value);
            await _cacheRepository.SetAsync(cacheKey, CacheValue, timeToLive);
        }
    }
}
