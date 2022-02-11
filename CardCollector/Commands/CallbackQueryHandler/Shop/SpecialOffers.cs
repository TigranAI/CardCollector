using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes.Menu;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Shop
{
    [MenuPoint]
    public class SpecialOffers : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.special_offers;
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            var specialOffers = await Context.SpecialOrders.FindAll();
            var availableOffers = specialOffers
                .Where(item => item.IsInfinite 
                               || !User.SpecialOrdersUser.Any(usedOrder => usedOrder.Order.Id == item.Id))
                .ToList();
            if (availableOffers.Count < 1)
                await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.offers_not_found, true);
            else
                await User.Messages.EditMessage(User, Messages.available_offers, Keyboard.SpecialOrdersKeyboard(availableOffers));
        }

        public SpecialOffers(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context,
            callbackQuery)
        {
        }
    }
}