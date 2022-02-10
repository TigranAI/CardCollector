using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.Resources;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Shop
{
    public class SelectProvider : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.select_provider;
        protected override bool AddToStack => true;

        protected override async Task Execute()
        {
            await User.Messages.EditMessage(User, Messages.choose_provider, Keyboard.ProviderKeyboard);
        }

        public SelectProvider(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery)
        {
        }
    }
}