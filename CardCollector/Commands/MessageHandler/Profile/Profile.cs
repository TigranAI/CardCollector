using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Database.EntityDao;
using CardCollector.Extensions.Database.Entity;
using CardCollector.Resources;

namespace CardCollector.Commands.MessageHandler.Profile
{
    [MenuPoint]
    public class Profile : MessageHandler
    {
        protected override string CommandText => MessageCommands.profile;
        protected override bool ClearMenu => true;
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            await User.Messages.ClearChat();
            User.Session.ResetModules();
            var income = User.Cash.GetIncome(User.Stickers);
            var currentLevel = await Context.Levels.FindLevel(User.Level.Level + 1);
            var expGoal = currentLevel?.LevelExpGoal.ToString() ?? "∞";
            var packsCount = User.Packs.Sum(item => item.Count);

            await User.Messages.SendMessage(User.GetProfileMessage(expGoal),
                Keyboard.GetProfileKeyboard(packsCount, User.InviteInfo, income));
        }
    }
}