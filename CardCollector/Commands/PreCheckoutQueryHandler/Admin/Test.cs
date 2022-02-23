using System.Threading.Tasks;
using CardCollector.Database;
using Telegram.Bot;
using Telegram.Bot.Types.Payments;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.PreCheckoutQueryHandler.Admin
{
    public class Test : PreCheckoutQueryHandler
    {
        protected override string CommandText => "test";

        protected override async Task Execute()
        {
            await Bot.Client.AnswerPreCheckoutQueryAsync(PreCheckoutQuery.Id);
        }

        public Test(User user, BotDatabaseContext context, PreCheckoutQuery preCheckoutQuery) : base(user, context, preCheckoutQuery)
        {
        }
    }
}