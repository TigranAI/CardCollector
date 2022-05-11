using CardCollector.Cache.Entity;

namespace CardCollector.Cache.Repository
{
    public class UserInfoRepository : RedisRepository<long, UserInfo>
    {
        protected override int DbNum => 0;
    }
}