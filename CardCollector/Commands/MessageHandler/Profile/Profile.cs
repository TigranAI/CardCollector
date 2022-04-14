using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes.Menu;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Translations;

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
            User.Session.ResetModules();
            var income = User.Cash.GetIncome(User.Stickers);
            var currentLevel = await Context.Levels.FindLevel(User.Level.Level + 1);
            var expGoal = currentLevel?.LevelExpGoal.ToString() ?? "∞";
            var packsCount = User.Packs.Sum(item => item.Count);

            var message = $"{User.Username}" +
                          $"\n{Messages.coins}: {User.Cash.Coins}{Text.coin}" +
                          $"\n{Messages.gems}: {User.Cash.Gems}{Text.gem}" +
                          $"\n{Messages.level}: {User.Level.Level}" +
                          $"\n{Messages.current_exp}: {User.Level.CurrentExp} / {expGoal}" +
                          $"\n{Messages.cash_capacity}: {User.Cash.MaxCapacity}{Text.coin}";
            if (User.InviteInfo?.ShowInvitedBy() is true)
                message += $"\n{Messages.inviter}: {User.InviteInfo.InvitedBy!.Username}";
            message += $"\n{Messages.see_your_stickers}";
            
            await User.Messages.EditMessage(User, message, Keyboard.GetProfileKeyboard(packsCount, User.InviteInfo, income));
        }
    }
}