using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Database.Entity;
using CardCollector.Resources.Translations;

namespace CardCollector.Others;

public struct TopReward
{
    public int Gems;
    public int Exp;
    public int Coins;

    public TopReward(int gems, int exp, int coins)
    {
        Gems = gems;
        Exp = exp;
        Coins = coins;
    }

    public async Task RewardUser(BotDatabaseContext context, User user)
    {
        await user.AddGems(Gems);
        user.Cash.Coins += Coins;
        user.Level.GiveExp(Exp);
        await user.Level.CheckLevelUp(context);
    }

    public override string ToString()
    {
        return string.Format(Text.top_reward, Gems, Exp, Coins);
    }
}