using System.Threading.Tasks;
using CardCollector.Resources;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.CallbackQueryHandler.Filters
{
    public class SelectTier : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.tier;

        protected override async Task Execute()
        {
            await User.Messages.EditMessage(User, Messages.choose_tier, Keyboard.TierOptions);
        }
    }
}