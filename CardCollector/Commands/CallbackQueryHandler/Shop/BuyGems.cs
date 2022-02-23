using System.Threading.Tasks;
using CardCollector.Attributes.Menu;
using CardCollector.Database;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Shop
{
    [MenuPoint]
    public class BuyGems : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.buy_gems;

        protected override async Task Execute()
        {
            User.Session.GetModule<ShopModule>().SelectedProvider = CallbackQuery.Data!.Split("=")[1];
            await User.Messages.EditMessage(User, Messages.buy_gems, Keyboard.BuyGems);
        }

        public BuyGems(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery)
        {
        }
    }
}