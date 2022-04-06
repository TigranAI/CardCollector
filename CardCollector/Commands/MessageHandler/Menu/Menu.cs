using System.Threading.Tasks;

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
    }
}