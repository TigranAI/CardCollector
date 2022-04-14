using System.Threading.Tasks;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Translations;

namespace CardCollector.Database.Entity
{
    public class UserLevel
    {
        public int Level { get; set; }
        public long CurrentExp { get; set; }
        public long TotalExp { get; set; }

        public void GiveExp(int expCount)
        {
            CurrentExp += expCount;
            TotalExp += expCount;
        }

        public async Task CheckLevelUp(BotDatabaseContext context, User user)
        {
            var levelInfo = await context.Levels.FindLevel(Level + 1);
            while (levelInfo?.LevelExpGoal <= CurrentExp)
            {
                CurrentExp -= levelInfo.LevelExpGoal;
                Level++;
                var levelReward = levelInfo.LevelReward;
                var rewardMessage = await levelReward.GetReward(context, user);
                var message = $"{Messages.congratulation_new_level} {Level}\n{rewardMessage}";
                await user.Messages.SendMessage(user, message, levelReward.RandomPacks > 0 ? Keyboard.MyPacks : null);
                levelInfo = await context.Levels.FindLevel(Level + 1);
            }
        }
    }
}