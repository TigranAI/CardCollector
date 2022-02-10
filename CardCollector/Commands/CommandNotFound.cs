using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands
{
    public class CommandNotFound : HandlerModel
    {
        protected override string CommandText => "";

        private readonly string _command;

        protected override async Task Execute()
        {
            await User.Messages.EditMessage(User, "Команда не найдена " + _command);
        }

        public override bool Match() => true;

        public CommandNotFound(User user, BotDatabaseContext context, string command) : base(user, context)
        {
            _command = command;
        }
    }
}