using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message.TextMessage
{
    public class Menu : MessageCommand
    {
        protected override string CommandText => Text.menu;
        public override async Task Execute()
        {
            /* Отправляем пользователю сообщение со стандартной клавиатурой */
            var message = await MessageController.SendMessage(User, Messages.menu_message, Keyboard.Menu);
            User.Session.Messages.Add(message.MessageId);
        }

        public Menu() { }
        public Menu(UserEntity user, Update update) : base(user, update) { }
    }
}