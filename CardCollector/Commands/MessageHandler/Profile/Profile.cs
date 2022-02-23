using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes.Menu;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

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
            var income = User.Cash.GetIncome(User.Stickers);
            var currentLevel = await Context.Levels.FindLevel(User.Level.Level + 1);
            var expGoal = currentLevel?.LevelExpGoal.ToString() ?? "∞";
            var packsCount = User.Packs.Sum(item => item.Count);
            await User.Messages.EditMessage(User,
                $"{User.Username}" +
                $"\n{Messages.coins}: {User.Cash.Coins}{Text.coin}" +
                $"\n{Messages.gems}: {User.Cash.Gems}{Text.gem}" +
                $"\n{Messages.level}: {User.Level.Level}" +
                $"\n{Messages.current_exp}: {User.Level.CurrentExp} / {expGoal}" +
                $"\n{Messages.cash_capacity}: {User.Cash.MaxCapacity}{Text.coin}" +
                $"\n{Messages.see_your_stickers}",
                Keyboard.GetProfileKeyboard(packsCount, income));
        }

        public Profile(User user, BotDatabaseContext context, Message message) : base(user, context, message)
        {
        }
    }
}