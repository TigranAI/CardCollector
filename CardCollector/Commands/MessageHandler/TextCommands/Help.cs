using System.Threading.Tasks;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.MessageHandler.TextCommands
{
    public class Help : MessageHandler
    {
        protected override string CommandText => MessageCommands.help;
        protected override async Task Execute()
        {
            await User.Messages.EditMessage(User, Messages.help);
        }
    }
}