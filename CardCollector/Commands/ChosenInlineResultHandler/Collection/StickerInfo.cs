using System.Linq;
using System.Threading.Tasks;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Enums;

namespace CardCollector.Commands.ChosenInlineResultHandler.Collection
{
    public class StickerInfo : ChosenInlineResultHandler
    {
        protected override string CommandText => ChosenInlineResultCommands.sticker_info;

        protected override async Task Execute()
        {
            var stickerId = long.Parse(ChosenInlineResult.ResultId.Split('=')[1]);
            var userSticker = User.Stickers.Single(item => item.Sticker.Id == stickerId);
            await User.Messages.ClearChat();
            await User.Messages.SendSticker(userSticker.Sticker.FileId);
            await User.Messages.SendMessage(userSticker.ToString(),
                Keyboard.StickerInfoKeyboard);
        }

        public override bool Match()
        {
            return base.Match() &&
                   User.Session.State is not (UserState.ShopMenu or UserState.AuctionMenu or UserState.CreateGiveaway);
        }
    }
}