using System.Threading.Tasks;
using CardCollector.DataBase;
using Telegram.Bot;
using Telegram.Bot.Types.Payments;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.PreCheckoutQueryHandler.Shop
{
    public class BuyGems : PreCheckoutQueryHandler
    {
        protected override string CommandText => PreCheckoutQueryCommands.buy_gems_item;

        protected override async Task Execute()
        {
            await Bot.Client.AnswerPreCheckoutQueryAsync(PreCheckoutQuery.Id);
        }

        public BuyGems(User user, BotDatabaseContext context, PreCheckoutQuery preCheckoutQuery) : base(user, context, preCheckoutQuery)
        {
        }
    }
}