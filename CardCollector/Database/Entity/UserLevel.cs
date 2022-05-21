using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Translations;

namespace CardCollector.Database.Entity;

[Table("user_level")]
public class UserLevel
{
    [Key, ForeignKey("id")]
    public virtual User User { get; set; }
    public int Level { get; set; }
    public long CurrentExp { get; set; }
    public long TotalExp { get; set; }

    public void GiveExp(int expCount)
    {
        User.UserStats.GainExp(expCount);
        CurrentExp += expCount;
        TotalExp += expCount;
    }

    public async Task CheckLevelUp(BotDatabaseContext context)
    {
        var levelInfo = await context.Levels.FindLevel(Level + 1);
        while (levelInfo?.LevelExpGoal <= CurrentExp)
        {
            CurrentExp -= levelInfo.LevelExpGoal;
            Level++;
            var levelReward = levelInfo.LevelReward;
            var rewardMessage = await levelReward.GetReward(context, User);
            var message = $"{Messages.congratulation_new_level} {Level}\n{rewardMessage}";
            await User.Messages.SendMessage(message, levelReward.RandomPacks > 0 ? Keyboard.MyPacks : null);
            levelInfo = await context.Levels.FindLevel(Level + 1);
        }
    }
}