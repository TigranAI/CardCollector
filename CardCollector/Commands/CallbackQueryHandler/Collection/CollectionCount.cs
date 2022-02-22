using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Collection
{
    public class CollectionCount : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.count;

        protected override async Task Execute()
        {
            var module = User.Session.GetModule<CollectionModule>();
            var userSticker = User.Stickers.FirstOrDefault(item => item.Sticker.Id == module.SelectedStickerId);
            if (CallbackQuery.Data!.Contains(Text.plus))
            {
                if (module.Count < userSticker?.Count) module.Count++;
                else await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.cant_change_count);
            }
            else if (CallbackQuery.Data!.Contains(Text.minus))
            {
                if (module.Count > 1) module.Count--;
                else await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.cant_change_count);
            }

            await MessageController.EditReplyMarkup(User, CallbackQuery.Message!.MessageId,
                Keyboard.GetCollectionStickerKeyboard(userSticker!.Sticker, module.Count));
        }

        public override bool Match()
        {
            return base.Match() && User.Session.State is UserState.CollectionMenu;
        }

        public CollectionCount(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context,
            callbackQuery)
        {
        }
    }
}