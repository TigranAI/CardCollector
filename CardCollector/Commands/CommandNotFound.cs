using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using Telegram.Bot.Types;

namespace CardCollector.Commands
{
    public class CommandNotFound : HandlerModel
    {
        protected override string CommandText => "";

        private readonly string _command;

        public override async Task Execute()
        {
            await MessageController.EditMessage(User, "Команда не найдена " + _command);
        }

        protected internal override bool Match(UserEntity user, Update update) => true;

        public CommandNotFound(UserEntity user, Update update, string command) : base(user, update)
        {
            _command = command;
        }
    }
}