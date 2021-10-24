using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message
{
    /* Обработка команды "/start" */
    public class Start : MessageCommand
    {
        /* */
        protected override string CommandText => Text.start;
        protected override bool ClearMenu => false;
        protected override bool AddToStack => false;

        public override async Task Execute()
        {
            /* Отправляем пользователю сообщение со стандартной клавиатурой */
            await MessageController.SendMessage(User, Messages.start_message, Keyboard.Menu);
        }
        
        public Start() { }
        public Start(UserEntity user, Update update) : base(user, update) { }
    }
}