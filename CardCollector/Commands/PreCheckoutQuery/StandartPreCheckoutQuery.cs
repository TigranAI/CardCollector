using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CardCollector.Commands.PreCheckoutQuery
{
    public class StandartPreCheckoutQuery : PreCheckoutQuery
    {
        protected override string CommandText => "";

        public override async Task Execute()
        {
            await Bot.Client.AnswerPreCheckoutQueryAsync(PreCheckoutQueryId);
        }

        public StandartPreCheckoutQuery() { }
        public StandartPreCheckoutQuery(UserEntity user, Update update) : base(user, update) { }
    }
}