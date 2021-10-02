using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CardCollector.Commands.PreCheckoutQuery
{
    public class TestPreCheckoutQuery : PreCheckoutQuery
    {
        protected override string CommandText => "test";

        public override async Task Execute()
        {
            await Bot.Client.AnswerPreCheckoutQueryAsync(PreCheckoutQueryId);
        }

        public TestPreCheckoutQuery() { }
        public TestPreCheckoutQuery(UserEntity user, Update update) : base(user, update) { }
    }
}