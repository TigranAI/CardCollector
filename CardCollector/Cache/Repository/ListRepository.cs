using System;

namespace CardCollector.Cache.Repository
{
    public class ListRepository<T> : RedisListRepository<string, T> where T : IConvertible
    {
        protected override int DbNum => 10;
    }
}