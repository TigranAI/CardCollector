using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.Resources;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Shop
{
    public class BuyPack : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.buy_pack;
        protected override bool AddToStack => true;
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