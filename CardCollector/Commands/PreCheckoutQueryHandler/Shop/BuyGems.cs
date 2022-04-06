using System.Threading.Tasks;
using Telegram.Bot;

namespace CardCollector.Commands.PreCheckoutQueryHandler.Shop
{
    public class BuyGems : PreCheckoutQueryHandler
    {
        protected override string CommandText => PreCheckoutQueryCommands.buy_gems_item;

        protected override async Task Execute()
        {
            await Bot.Client.AnswerPreCheckoutQueryAsync(PreCheckoutQuery.Id);
        }
    }
}