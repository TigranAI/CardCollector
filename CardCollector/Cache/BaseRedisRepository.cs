using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CardCollector.Others;
using StackExchange.Redis;

namespace CardCollector.Cache
{
    public abstract class BaseRedisRepository<TK, TV>
        where TK : notnull
    {
        protected readonly IDatabase Database;

        protected BaseRedisRepository(int dbNum)
        {
            Database = RedisCache.Connection.GetDatabase(dbNum);
        }

        protected abstract TV GetDefault();
        protected abstract string GetKey(TK key);

        public async Task<TV> GetAsync([NotNull] TK key)
        {
            var redisResult = await Database.StringGetAsync(GetKey(key));
            return redisResult.HasValue
                ? Utilities.FromJson<TV>(redisResult)
                : GetDefault();
        }

        public async Task<bool> SaveAsync([NotNull] TK key, [NotNull] TV value)
        {
            ThrowHelper.CheckNonNull(value);
            return await Database.StringSetAsync(GetKey(key), Utilities.ToJson(value!));
        }

        public Task<bool> DeleteAsync([NotNull] TK key)
        {
            return Database.KeyDeleteAsync(GetKey(key));
        }

        public Task ClearAsync()
        {
            return Database.ExecuteAsync("FLUSHDB");
        }
    }
}