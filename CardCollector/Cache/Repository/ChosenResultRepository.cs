namespace CardCollector.Cache.Repository
{
    public class ChosenResultRepository : RedisRepository<long, long?>
    {
        protected override int DbNum => 11;
    }
}