using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Database.Entity;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.CallbackQueryHandler.Profile
{
    [MenuPoint]
    public class InviteFriend : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.invite_friend;
        protected override async Task Execute()
        {
            if (User.InviteInfo == null)
            {
                User.InviteInfo = new InviteInfo();
                User.InviteInfo.InviteKey = await InviteInfo.GenerateKey();
            }

            var url = User.InviteInfo.GetTelegramUrl();
            
            await User.Messages.EditMessage(User, string.Format(Messages.your_invite_link, url),
                Keyboard.InviteMenu(url), ParseMode.Html);
        }
    }
}