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
            var stickerCount = sticker.TraderInfo?.Quantity ?? sticker.Count;
            var text = $"\n<<{sticker.Title}>>" +
                       $"\n{Text.emoji}: {sticker.Emoji}" +
                       $"\n{Text.author}: {sticker.Author}" +
                       $"\n{Text.count}: {(stickerCount != -1 ? stickerCount : "∞")}" +
                       $"\n{sticker.IncomeCoins}{Text.coin} / {sticker.IncomeGems}{Text.gem} {sticker.IncomeTime}{Text.time}{Text.minutes}";
            await MessageController.EditMessage(User, CallbackMessageId, text, Keyboard.GetStickerKeyboard(User.Session));
        }

        public BackToStickerQuery() { }
        public BackToStickerQuery(UserEntity user, Update update) : base(user, update) { }
    }
}