using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Telegram.Bot.Types;

namespace CardCollector.Commands
{
    public class IgnoreHandler : HandlerModel
    {
        protected override string CommandText => "";

        public override Task PrepareAndExecute() => Task.CompletedTask;
        public override Task Execute() => Task.CompletedTask;

        protected internal override bool Match(UserEntity user, Update update) => true;

        public IgnoreHandler() : base(null, null)
        {
        }
    }
}