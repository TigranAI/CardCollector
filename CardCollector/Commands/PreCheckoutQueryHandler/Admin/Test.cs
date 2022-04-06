using System.Threading.Tasks;
using Telegram.Bot;

namespace CardCollector.Commands.PreCheckoutQueryHandler.Admin
{
    public class Test : PreCheckoutQueryHandler
    {
        protected override string CommandText => "test";

        protected override async Task Execute()
        {
            await Bot.Client.AnswerPreCheckoutQueryAsync(PreCheckoutQuery.Id);
        }
    }
}