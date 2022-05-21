using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Extensions.Database.Entity;
using CardCollector.Resources;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.CallbackQueryHandler.Profile
{
    [Statistics]
    public class CollectIncome : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.collect_income;

        protected override async Task Execute()
        {
            var result = await User.Cash.Payout();
            await AnswerCallbackQuery(User, CallbackQuery.Id,
                $"{Messages.you_collected} {result} " +
                $"\n\n{Messages.your_cash}: {User.Cash}", true);
            var currentLevel = await Context.Levels.FindLevel(User.Level.Level + 1);
            var expGoal = currentLevel?.LevelExpGoal.ToString() ?? "∞";
            await User.Messages.EditMessage(User.GetProfileMessage(expGoal),
                Keyboard.GetProfileKeyboard(User.Packs.Sum(pack => pack.Count), User.InviteInfo));

            if (User.InviteInfo?.TasksProgress is { } tp && tp.CollectIncome < BeginnersTasksProgress.CollectIncomeGoal)
            {
                tp.CollectIncome++;
                await User.InviteInfo.CheckRewards(Context);
            }
        }
    }
}