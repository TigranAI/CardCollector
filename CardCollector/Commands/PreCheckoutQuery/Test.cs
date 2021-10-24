using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CardCollector.Commands.PreCheckoutQuery
{
    public class Test : PreCheckoutQueryCommand
    {
        protected override string CommandText => "test";
        protected override bool ClearMenu => false;
        protected override bool AddToStack => false;

        public override async Task Execute()
        {
            await Bot.Client.AnswerPreCheckoutQueryAsync(PreCheckoutQueryId);
        }

        public Test() { }
        public Test(UserEntity user, Update update) : base(user, update) { }
    }
}