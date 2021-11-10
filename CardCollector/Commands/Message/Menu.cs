using System.Threading.Tasks;
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
            await User.ClearChat();
            await User.MessagesId.SendMenu();/*
            /* Отправляем пользователю сообщение со стандартной клавиатурой #1#
            await MessageController.SendMessage(User, Messages.main_menu, Keyboard.Menu, ParseMode.Html);*/
        }

        public Menu() { }
        public Menu(UserEntity user, Update update) : base(user, update) { }
    }
}