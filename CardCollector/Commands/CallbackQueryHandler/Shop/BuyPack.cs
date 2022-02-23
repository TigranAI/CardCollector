using System.Threading.Tasks;
using CardCollector.Attributes.Menu;
using CardCollector.Database;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Shop
{
    [MenuPoint]
    public class BuyPack : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.buy_pack;
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            await User.Messages.EditMessage(User, Messages.choose_option, Keyboard.ShopPacksKeyboard);
        }

        public BuyPack(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery)
        {
        }
    }
}