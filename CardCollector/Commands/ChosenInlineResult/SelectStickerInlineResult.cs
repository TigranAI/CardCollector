using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Others;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.ChosenInlineResult
{
    public class SelectStickerInlineResult : ChosenInlineResult
    {
        protected override string CommandText => Command.select_sticker;
        public override async Task Execute()
        {
            await User.ClearChat();
            var hash = InlineResult.Split('=')[1];
            var sticker = await StickerDao.GetStickerByHash(hash);
            var stickerCount = User.Session.State switch
            {
                UserState.AuctionMenu => await AuctionController.GetStickerCount(sticker.Id),
                UserState.ShopMenu => await ShopController.GetStickerCount(sticker.Id),
                _ => User.Stickers[sticker.Md5Hash].Count
            };
            var messageText = string.Concat(Enumerable.Repeat(Text.star, sticker.Tier));
            messageText += $"\n<<{sticker.Title}>>" +
                           $"\n{Text.emoji}: {sticker.Emoji}" +
                           $"\n{Text.author}: {sticker.Author}" +
                           $"\n{Text.count}: {(stickerCount != -1 ? stickerCount : "∞")}" +
                           $"\n{sticker.IncomeCoins}{Text.coin} / {sticker.IncomeGems}{Text.gem} {sticker.IncomeTime}{Text.time}{Text.minutes}";
            if (sticker.Description != "") messageText += $"\n\n{Text.description}: {sticker.Description}";
            var stickerMessage = await MessageController.SendSticker(User, sticker.Id);
            var stickerInfo = new StickerInfo(sticker) {Count = 1};
            User.Session.SelectedSticker = stickerInfo;
            var infoMessage = await MessageController.SendMessage(User, messageText, Keyboard.GetStickerKeyboard(stickerInfo, User.Session.State));
            User.Session.Messages.Add(stickerMessage.MessageId);
            User.Session.Messages.Add(infoMessage.MessageId);
        }

        public SelectStickerInlineResult() { }
        public SelectStickerInlineResult(UserEntity user, Update update, string inlineResult) : base(user, update, inlineResult) { }
    }
}