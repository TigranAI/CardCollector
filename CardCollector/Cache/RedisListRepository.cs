using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace CardCollector.Cache
{
    public abstract class RedisListRepository<TK, TV> where TK : IConvertible where TV : IConvertible
    {
        protected abstract int DbNum { get; }
        protected IDatabase Database => RedisCache.Connection.GetDatabase(DbNum);

        public Task<bool> AddAsync([NotNull] TK key, [NotNull] TV value)
        {
            return Database.SetAddAsync(key.ToString(), value.ToString());
        }

        public Task<bool> RemoveAsync([NotNull] TK key, [NotNull] TV value)
        {
            return Database.SetRemoveAsync(key.ToString(), value.ToString());
        }

        public Task<bool> ContainsAsync([NotNull] TK key, [NotNull] TV value)
        {
            return Database.SetContainsAsync(key.ToString(), value.ToString());
        }
    }
}