using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.Resources;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.MessageHandler.Menu
{
    public class Menu : MessageHandler
    {
        protected override string CommandText => MessageCommands.menu;

        protected override async Task Execute()
        {
            await User.Messages.ClearChat(User);
            await User.Messages.SendMenu(User);
        }

        public Menu(User user, BotDatabaseContext context, Message message) : base(user, context, message)
        {
        }
    }
}