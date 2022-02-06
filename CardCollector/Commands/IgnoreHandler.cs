using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.DataBase.Entity;

namespace CardCollector.Commands
{
    public class IgnoreHandler : HandlerModel
    {
        protected override string CommandText => "";
        protected override Task Execute() => Task.CompletedTask;
        public override bool Match() => false;
        public IgnoreHandler(User user, BotDatabaseContext context) : base(user, context)
        {
        }
    }
}