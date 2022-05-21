using System.Threading.Tasks;
using CardCollector.Resources;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.MessageHandler.UrlCommands
{
    public class Login : MessageUrlHandler
    {
        protected override string CommandText => MessageUrlCommands.confirm_login;

        protected override async Task Execute()
        {
            await User.Messages.EditMessage($"{Messages.confirm_login} {AppSettings.SITE_URL}",
                Keyboard.ConfirmLogin(StartData[1]));
        }
    }
}