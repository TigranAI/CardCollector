using CardCollector.Cache.Entity;
using CardCollector.Database.Entity;

namespace CardCollector.Cache.Repository
{
    public class UserInfoRepository : BaseRedisRepository<User, UserInfo>
    {
        public UserInfoRepository() : base(0)
        {
        }

        protected override UserInfo GetDefault()
        {
            return new UserInfo();
        }

        protected override string GetKey(User key)
        {
            return key.Id.ToString();
        }
    }
}