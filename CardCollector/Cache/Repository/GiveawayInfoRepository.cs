using CardCollector.Cache.Entity;

namespace CardCollector.Cache.Repository
{
    public class GiveawayInfoRepository : RedisRepository<long, GiveawayInfo>
    {
        protected override int DbNum => 2;
    }
}