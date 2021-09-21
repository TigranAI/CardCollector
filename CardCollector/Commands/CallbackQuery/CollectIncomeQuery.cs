using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class CollectIncomeQuery : CallbackQuery
    {
        protected override string CommandText => Command.collect_income;
        public override async Task Execute()
        {
            await MessageController.DeleteMessage(User, Update.CallbackQuery!.Message!.MessageId);
            await User.Session.PayOut();
            await Bot.Client.AnswerCallbackQueryAsync(Update.CallbackQuery.Id, 
                $"{Messages.you_collected}: " +
                $"{User.Session.IncomeCoins}{Text.coin} " +
                $"{User.Session.IncomeGems}{Text.gem}" +
                $"\n\n{Messages.your_cash}: " +
                $"{User.Cash.Coins}{Text.coin} " +
                $"{User.Cash.Gems}{Text.gem}", true);
        }

        public CollectIncomeQuery() { }

        public CollectIncomeQuery(UserEntity user, Update update) : base(user, update) { }
    }
}