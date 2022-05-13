using CardCollector.Cache.Entity;

namespace CardCollector.Cache.Repository
{
    public class ChatInfoRepository : RedisRepository<long, ChatInfo>
    {
        protected override int DbNum => 3;
    }
}