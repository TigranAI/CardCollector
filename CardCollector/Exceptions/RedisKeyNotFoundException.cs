using System.Collections.Generic;

namespace CardCollector.Exceptions
{
    public class RedisKeyNotFoundException : KeyNotFoundException
    {
        private const string MSG = "Key {0} not found in redis repository";
        
        public RedisKeyNotFoundException(object key) : base(string.Format(MSG, key))
        {
        }
    }
}