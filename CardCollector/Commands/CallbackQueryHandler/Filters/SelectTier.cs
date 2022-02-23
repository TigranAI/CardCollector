using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.Database;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Filters
{
    public class SelectTier : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.tier;

        protected override async Task Execute()
        {
            await User.Messages.EditMessage(User, Messages.choose_tier, Keyboard.TierOptions);
        }
        
        public SelectTier(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery) { }
    }
}