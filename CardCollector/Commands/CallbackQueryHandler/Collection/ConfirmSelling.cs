using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes.Logs;
using CardCollector.Commands.MessageHandler.Collection;
using CardCollector.Controllers;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Collection
{
    [SavedActivity]
    public class ConfirmSelling : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.confirm_selling;
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            var collectionModule = User.Session.GetModule<CollectionModule>();
            if (collectionModule.SellPrice <= 0)
                await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.cant_sell_zero, true);
            else
            {
                EnterGemsPrice.RemoveFromQueue(User.Id);
                var userSticker =
                    User.Stickers.SingleOrDefault(item => item.Sticker.Id == collectionModule.SelectedStickerId);
                if (userSticker == null) return;
                userSticker.Count -= collectionModule.Count;
                await Context.Auctions.AddAsync(User, userSticker.Sticker, collectionModule.Count,
                    collectionModule.SellPrice);
                await User.Messages.EditMessage(User, Messages.successfully_selling);
            }
        }
        
        public ConfirmSelling(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery){}
    }
}