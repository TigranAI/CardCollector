using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.Message
{
    public class Menu : MessageCommand
    {
        protected override string CommandText => Text.menu;

        public override async Task Execute()
        {
            await User.ClearChat();
            User.MessagesId.MenuId = await MessageController.DeleteAndSend(User, User.MessagesId.MenuId,
                Messages.main_menu, Keyboard.Menu, ParseMode.Html);
        }

        public Menu() { }
        public Menu(UserEntity user, Update update) : base(user, update) { }
    }
}