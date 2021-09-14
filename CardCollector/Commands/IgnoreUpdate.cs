using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands
{
    /* Данный класс позволяет проигнорировать действие пользователя */
    public class IgnoreUpdate : UpdateModel
    {
        protected override string Command => "";
        public override async Task Execute()
        {
            if (Update.Message?.Chat.Type is ChatType.Private)
                await MessageController.DeleteMessage(User, Update.Message.MessageId);
        }
        
        public IgnoreUpdate(UserEntity user, Update update) : base(user, update) { }
        public IgnoreUpdate() { }
    }
}