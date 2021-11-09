using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class CollectIncome : CallbackQueryCommand
    {
        protected override string CommandText => Command.collect_income;

        public override async Task Execute()
        {
            var result = await User.Cash.Payout(User.Stickers);
            await MessageController.AnswerCallbackQuery(User, CallbackQueryId, 
                $"{Messages.you_collected} {result}{Text.coin} " +
                $"\n\n{Messages.your_cash}: {User.Cash.Coins}{Text.coin} {User.Cash.Gems}{Text.gem}", true);
            var expGoal = (await LevelDao.GetLevel(User.CurrentLevel.Level + 1))?.LevelExpGoal.ToString() ?? "∞";
            var packsCount = await UserPacksDao.GetCount(User.Id);
            /* Отправляем сообщение */
            await MessageController.EditMessage(User, 
                $"{User.Username}" +
                $"\n{Messages.coins}: {User.Cash.Coins}{Text.coin}" +
                $"\n{Messages.gems}: {User.Cash.Gems}{Text.gem}" +
                $"\n{Messages.level}: {User.CurrentLevel.Level}" +
                $"\n{Messages.current_exp}: {User.CurrentLevel.CurrentExp} / {expGoal}" +
                $"\n{Messages.cash_capacity}: {User.Cash.MaxCapacity}{Text.coin}",
                Keyboard.GetProfileKeyboard(User.PrivilegeLevel, packsCount));
        }

        public CollectIncome() { }
        public CollectIncome(UserEntity user, Update update) : base(user, update) { }
    }
}