using System.Threading.Tasks;
using CardCollector.Commands.MessageHandler.Collection;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Collection
{
    public class SellSticker : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.sell_sticker;

        protected override async Task Execute()
        {
            await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.comission_warning, true);
            var module = User.Session.GetModule<CollectionModule>();
            var lowerPrice = Context.Auctions.FindMinPriceByStickerId(module.SelectedStickerId);
            await User.Messages.EditMessage(User,
                $"{Messages.current_price} {module.SellPrice}{Text.gem}" +
                $"\n{Messages.lower_price} {lowerPrice}{Text.gem}" +
                $"\n{Messages.enter_your_gems_price} {Text.gem}:", Keyboard.BackKeyboard);
            EnterGemsPrice.AddToQueue(User.Id);
        }

        public SellSticker(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery) { }
    }
}