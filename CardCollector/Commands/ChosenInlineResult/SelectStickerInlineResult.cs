using System.Linq;
using System.Threading.Tasks;
using CardCollector.Auction;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
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
            var stickerCount = User.State switch
            {
                UserState.AuctionMenu => await AuctionController.GetStickerCount(sticker.Id),
                UserState.ShopMenu => await ShopController.GetStickerCount(sticker.Id),
                _ => User.Stickers[sticker.Md5Hash].Count
            };
            var messageText = string.Concat(Enumerable.Repeat(Text.star, sticker.Tier));
            messageText += $"\n<<{sticker.Title}>>" +
                           $"\n{Text.emoji}: {sticker.Emoji}" +
                           $"\n{Text.author}: {sticker.Author}" +
                           $"\n{Text.count}: {stickerCount}" +
                           $"\n{sticker.IncomeCoins}{Text.coin} / {sticker.IncomeGems}{Text.gem} {sticker.IncomeTime}{Text.time}{Text.minutes}";
            if (sticker.Description != "") messageText += $"\n\n{Text.description}: {sticker.Description}";
            var stickerMessage = await MessageController.SendSticker(User, sticker.Id);
            var infoMessage = await MessageController.SendMessage(User, messageText, Keyboard.GetStickerKeyboard(User.State, sticker.Md5Hash, sticker.Title));
            User.Messages.Add(stickerMessage.MessageId);
            User.Messages.Add(infoMessage.MessageId);
        }

        public SelectStickerInlineResult() { }
        public SelectStickerInlineResult(UserEntity user, Update update, string inlineResult) : base(user, update, inlineResult) { }
    }
}