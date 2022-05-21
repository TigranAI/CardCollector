using System.Threading.Tasks;
using CardCollector.Commands.MessageHandler.Collection;
using CardCollector.Controllers;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.CallbackQueryHandler.Collection
{
    public class SellSticker : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.sell_sticker;

        protected override async Task Execute()
        {
            await AnswerCallbackQuery(User, CallbackQuery.Id, Messages.comission_warning, true);
            var module = User.Session.GetModule<CollectionModule>();
            var lowerPrice = await Context.Auctions.FindMinPriceByStickerId(module.SelectedStickerId);
            await User.Messages.EditMessage(
                $"{Messages.current_price} {module.SellPrice}{Text.gem}" +
                $"\n{Messages.lower_price} {lowerPrice}{Text.gem}" +
                $"\n{Messages.enter_your_gems_price} {Text.gem}:", Keyboard.BackKeyboard);
            EnterGemsPrice.AddToQueue(User.Id);
        }
    }
}