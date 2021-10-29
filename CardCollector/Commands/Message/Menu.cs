using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message
{
    public class Menu : MessageCommand
    {
        protected override string CommandText => Text.menu;

        public override async Task Execute()
        {
            /* Отправляем пользователю сообщение со стандартной клавиатурой */
            await MessageController.SendMessage(User, Messages.menu_message, Keyboard.Menu);
        }

        public Menu() { }
        public Menu(UserEntity user, Update update) : base(user, update) { }
    }
}