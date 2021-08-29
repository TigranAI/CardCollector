using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using Telegram.Bot.Types;

namespace CardCollector.Commands.MessageCommands
{
    public class CommandNotFound : MessageCommand
    {
        protected override string Command => "";

        public override async Task<Message> Execute()
        {
            return await MessageController.SendMessage(User.ChatId, "Команда не найдена");
        }

        public CommandNotFound(UserEntity user) : base(user) { }
    }
}