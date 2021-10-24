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
        protected override bool ClearMenu => false;
        protected override bool AddToStack => false;

        private readonly string _command;

        public override async Task Execute()
        {
            var message = await MessageController.SendMessage(User, "Команда не найдена " + _command);
            User.Session.Messages.Add(message.MessageId);
        }

        protected internal override bool IsMatches(UserEntity user, Update update) => true;

        public CommandNotFound(UserEntity user, Update update, string command) : base(user, update)
        {
            _command = command;
        }
    }
}