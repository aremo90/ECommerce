using ECommerce.Domin.Contract;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Repository
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IDatabase _connection;

        public CacheRepository(IConnectionMultiplexer connection)
        {
            _connection = connection.GetDatabase();
        }


        public async Task<string?> GetAsync(string CacheKey)
        {
            var cacheValue = await _connection.StringGetAsync(CacheKey);
            if (cacheValue.IsNullOrEmpty) return null;
            return cacheValue;
        }

        public async Task SetAsync(string CacheKey, string CacheValue, TimeSpan TimeToLive)
        {
            await _connection.StringSetAsync(CacheKey, CacheValue, TimeToLive);
        }
    }
}
