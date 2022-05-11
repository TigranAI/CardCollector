using System;
using CardCollector.Resources;
using StackExchange.Redis;

namespace CardCollector.Cache
{
    public static class RedisCache
    {
        private static Lazy<ConnectionMultiplexer>? _lazyConnection;
        
        public static ConnectionMultiplexer Connection
        {
            get
            {
                if (_lazyConnection != null) return _lazyConnection.Value;
                _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
                {
                    return ConnectionMultiplexer.Connect(AppSettings.REDIS_CONNECTION_STRING);
                });
                return _lazyConnection.Value;
            }
        }
    }
}