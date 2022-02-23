using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes.Menu;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.MessageHandler.Shop
{
    [MenuPoint]
    public class Shop : MessageHandler
    {
        protected override string CommandText => MessageCommands.shop;
        protected override bool ClearMenu => true;
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            User.Session.State = UserState.ShopMenu;
            var availableSpecialOrders = await Context.SpecialOrders.FindAll();
            var haveSpecialOffers = availableSpecialOrders.Any(item => item.IsInfinite 
                || !User.SpecialOrdersUser.Any(usedOrders => usedOrders.Order.Id == item.Id));
            await User.Messages.EditMessage(User, Messages.shop_message,
                Keyboard.ShopKeyboard(haveSpecialOffers, User.PrivilegeLevel), ParseMode.Html);
        }

        public Shop(User user, BotDatabaseContext context, Message message) : base(user, context, message)
        {
        }
    }
}