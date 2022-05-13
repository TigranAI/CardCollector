using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CardCollector.Others;
using StackExchange.Redis;

namespace CardCollector.Cache
{
    public abstract class RedisRepository<TK, TV> where TK : IConvertible
    {
        protected abstract int DbNum { get; }
        protected IDatabase Database => RedisCache.Connection.GetDatabase(DbNum);

        public Task<bool> SaveAsync([NotNull] TK key, [NotNull] TV value)
        {
            ThrowHelper.CheckNonNull(value);
            return Database.StringSetAsync(key.ToString(), Utilities.ToJson(value));
        }

        public async Task<TV?> GetOrDefaultAsync([NotNull] TK key, TV? defaultValue = default)
        {
            var redisResult = await Database.StringGetAsync(key.ToString());
            return redisResult.HasValue
                ? Utilities.FromJson<TV>(redisResult)
                : defaultValue;
        }

        public Task<bool> DeleteAsync([NotNull] TK key)
        {
            return Database.KeyDeleteAsync(key.ToString());
        }

        public Task ClearAsync()
        {
            return Database.ExecuteAsync("FLUSHDB");
        }
    }
}