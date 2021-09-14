using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message
{
    /* Обработка команды "/start" */
    public class StartMessage : Message
    {
        /* */
        protected override string Command => MessageCommands.start;
        
        public override async Task Execute()
        {
            /* Отправляем пользователю сообщение со стандартной клавиатурой */
            await MessageController.SendMessage(User,"Привет!", Keyboard.Menu);
        }
        
        public StartMessage(UserEntity user, Update update) : base(user, update) { }
        public StartMessage() { }
    }
}