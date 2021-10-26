using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using Telegram.Bot.Types;

namespace CardCollector.Commands
{
    /* Данный класс реализует операцию "Команда не найдена" */
    public class CommandNotFound : UpdateModel
    {
        protected override string CommandText => "";

        private readonly string _command;

        public override async Task Execute()
        {
            await MessageController.SendMessage(User, "Команда не найдена " + _command);
        }

        protected internal override bool IsMatches(UserEntity user, Update update) => true;

        public CommandNotFound(UserEntity user, Update update, string command) : base(user, update)
        {
            _command = command;
        }
    }
}