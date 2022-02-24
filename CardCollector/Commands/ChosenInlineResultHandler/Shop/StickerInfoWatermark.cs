using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.ChosenInlineResultHandler.Shop
{
    public class StickerInfoWatermark : ChosenInlineResultHandler
    {
        protected override string CommandText => ChosenInlineResultCommands.sticker_info;
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            var stickerId = long.Parse(ChosenInlineResult.ResultId.Split('=')[1]);
            var sticker = await Context.Stickers.FindById(stickerId);
            await User.Messages.ClearChat(User);
            await User.Messages.SendSticker(User, sticker.ForSaleFileId ?? sticker.FileId);
            await User.Messages.SendMessage(User, sticker.ToString(), Keyboard.StickerInfoKeyboard);
        }

        public override bool Match()
        {
            return base.Match() && User.Session.State is UserState.ShopMenu or UserState.AuctionMenu;
        }

        public StickerInfoWatermark(User user, BotDatabaseContext context, ChosenInlineResult chosenInlineResult) :
            base(user, context, chosenInlineResult)
        {
        }
    }
}