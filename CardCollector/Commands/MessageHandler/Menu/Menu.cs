using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Resources;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

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