using CardCollector.Cache.Entity;
using CardCollector.Database.Entity;

namespace CardCollector.Cache.Repository
{
    public class ChatInfoRepository : BaseRedisRepository<TelegramChat, ChatInfo>
    {
        public ChatInfoRepository() : base(3)
        {
        }

        protected override ChatInfo GetDefault()
        {
            return new ChatInfo();
        }

        protected override string GetKey(TelegramChat key)
        {
            return key.Id.ToString();
        }
    }
}