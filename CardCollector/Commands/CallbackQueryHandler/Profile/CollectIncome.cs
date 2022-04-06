using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes.Logs;
using CardCollector.Controllers;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.CallbackQueryHandler.Profile
{
    [SavedActivity]
    public class CollectIncome : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.collect_income;

        protected override async Task Execute()
        {
            var result = User.Cash.Payout(User.Stickers);
            await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, 
                $"{Messages.you_collected} {result}{Text.coin} " +
                $"\n\n{Messages.your_cash}: {User.Cash.Coins}{Text.coin} {User.Cash.Gems}{Text.gem}", true);
            var currentLevel = await Context.Levels.FindLevel(User.Level.Level + 1);
            var expGoal = currentLevel?.LevelExpGoal.ToString() ?? "∞";
            await User.Messages.EditMessage(User, 
                $"{User.Username}" +
                $"\n{Messages.coins}: {User.Cash.Coins}{Text.coin}" +
                $"\n{Messages.gems}: {User.Cash.Gems}{Text.gem}" +
                $"\n{Messages.level}: {User.Level.Level}" +
                $"\n{Messages.current_exp}: {User.Level.CurrentExp} / {expGoal}" +
                $"\n{Messages.cash_capacity}: {User.Cash.MaxCapacity}{Text.coin}",
                Keyboard.GetProfileKeyboard(User.Packs.Sum(pack => pack.Count), User.InviteInfo));
        }
    }
}