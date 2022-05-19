using CardCollector.Cache.Entity;
using CardCollector.Database.Entity;

namespace CardCollector.Cache.Repository
{
    public class GiveawayInfoRepository : BaseRedisRepository<TelegramChat, GiveawayInfo>
    {
        public GiveawayInfoRepository() : base(2)
        {
        }

        protected override GiveawayInfo GetDefault()
        {
            return new GiveawayInfo();
        }

        protected override string GetKey(TelegramChat key)
        {
            return key.Id.ToString();
        }
    }
}