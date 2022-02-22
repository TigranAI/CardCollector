using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.ChosenInlineResultHandler.Collection
{
    public class StickerInfo : ChosenInlineResultHandler
    {
        protected override string CommandText => ChosenInlineResultCommands.sticker_info;

        protected override async Task Execute()
        {
            var stickerId = long.Parse(ChosenInlineResult.ResultId.Split('=')[1]);
            var sticker = await Context.Stickers.FindById(stickerId);
            var userSticker = User.Stickers.Single(item => item.Sticker.Id == stickerId);
            await User.Messages.SendSticker(User, sticker.FileId);
            await User.Messages.EditMessage(User, sticker.ToString(userSticker.Count),
                Keyboard.StickerInfoKeyboard);
        }

        public override bool Match()
        {
            return base.Match() &&
                   User.Session.State is not (UserState.ShopMenu or UserState.AuctionMenu or UserState.CreateGiveaway);
        }

        public StickerInfo(User user, BotDatabaseContext context, ChosenInlineResult chosenInlineResult) : base(user,
            context, chosenInlineResult)
        {
        }
    }
}