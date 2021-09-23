using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class BackToStickerQuery : CallbackQuery
    {
        protected override string CommandText => Command.back_to_sticker;
        public override async Task Execute()
        {
            var sticker = User.Session.SelectedSticker;
            await MessageController.EditMessage(User, CallbackMessageId, sticker.ToString(), Keyboard.GetStickerKeyboard(User.Session));
        }

        public BackToStickerQuery() { }
        public BackToStickerQuery(UserEntity user, Update update) : base(user, update) { }
    }
}