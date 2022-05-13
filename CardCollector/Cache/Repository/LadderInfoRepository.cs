using CardCollector.Cache.Entity;

namespace CardCollector.Cache.Repository
{
    public class LadderInfoRepository : RedisRepository<long, LadderInfo>
    {
        protected override int DbNum => 1;
    }
}