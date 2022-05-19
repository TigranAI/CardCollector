using CardCollector.Cache.Entity;
using CardCollector.Database.Entity;

namespace CardCollector.Cache.Repository
{
    public class LadderInfoRepository : BaseRedisRepository<TelegramChat, LadderInfo>
    {
        public LadderInfoRepository() : base(1)
        {
        }

        protected override LadderInfo GetDefault()
        {
            return new LadderInfo();
        }

        protected override string GetKey(TelegramChat key)
        {
            return key.Id.ToString();
        }
    }
}