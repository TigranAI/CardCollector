using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CardCollector.Commands.PreCheckoutQuery
{
    public class BuyGems : PreCheckoutQueryHandler
    {
        protected override string CommandText => Command.buy_gems_item;

        public override async Task Execute()
        {
            await Bot.Client.AnswerPreCheckoutQueryAsync(PreCheckoutQueryId);
        }

        public BuyGems(UserEntity user, Update update) : base(user, update) { }
    }
}