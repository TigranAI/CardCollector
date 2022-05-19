namespace CardCollector.Cache.Repository
{
    public class ChosenResultRepository : BaseRedisRepository<long, long?>
    {
        public ChosenResultRepository() : base(11)
        {
        }

        protected override long? GetDefault()
        {
            return null;
        }

        protected override string GetKey(long key)
        {
            return key.ToString();
        }
    }
}