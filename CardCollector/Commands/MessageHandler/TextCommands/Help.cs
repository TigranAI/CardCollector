using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.Resources;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.MessageHandler.TextCommands
{
    public class Help : MessageHandler
    {
        protected override string CommandText => Text.help;
        protected override async Task Execute()
        {
            await User.Messages.EditMessage(User, Messages.help);
        }

        public Help(User user, BotDatabaseContext context, Message message) : base(user, context, message)
        {
        }
    }
}