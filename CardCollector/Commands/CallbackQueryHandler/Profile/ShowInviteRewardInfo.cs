using System.Threading.Tasks;
using CardCollector.Resources;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.CallbackQueryHandler.Profile
{
    public class ShowInviteRewardInfo : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.show_invite_reward_info;
        protected override async Task Execute()
        {
            await User.Messages.EditMessage(Messages.invite_reward_info, Keyboard.BackKeyboard);
        }
    }
}