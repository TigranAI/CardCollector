using CardCollector.Cache.Entity;
using CardCollector.Database.Entity;

namespace CardCollector.Cache.Repository;

public class PuzzleInfoRepository : BaseRedisRepository<TelegramChat, PuzzleInfo>
{
    public PuzzleInfoRepository() : base(5)
    {
    }

    protected override PuzzleInfo GetDefault()
    {
        return new PuzzleInfo();
    }

    protected override string GetKey(TelegramChat key)
    {
        return key.Id.ToString();
    }
}