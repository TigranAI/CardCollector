using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace CardCollector.Cache
{
    public abstract class RedisRepository<TK, TV>
    {
        protected abstract int DbNum { get; }
        protected IDatabase Database => RedisCache.Connection.GetDatabase(DbNum);

        public Task<bool> SaveAsync(TK key, TV value)
        {
            if (key is null) throw new NullReferenceException("key must be initialized");
            if (value is null) throw new NullReferenceException("value must be initialized");
            return Database.StringSetAsync(key.ToString(), Utilities.ToJson(value));
        }

        public async Task<TV> GetAsync(TK key)
        {
            if (key is null) throw new NullReferenceException("key must be initialized");
            var redisResult = await Database.StringGetAsync(key.ToString());
            if (!redisResult.HasValue) throw new NullReferenceException($"key {key} not found");
            return Utilities.FromJson<TV>(redisResult);
        }
    }
}