using CardCollector.Database.Entity;

namespace CardCollector.Cache.Repository
{
    public class ChosenResultRepository : BaseRedisRepository<User, long?>
    {
        public ChosenResultRepository() : base(11)
        {
        }

        protected override long? GetDefault()
        {
            return null;
        }

        protected override string GetKey(User key)
        {
            return key.Id.ToString();
        }
    }
}