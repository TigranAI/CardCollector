using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Commands.ChosenInlineResultHandler;
using CardCollector.Controllers;
using CardCollector.Database.EntityDao;
using CardCollector.Others;
using CardCollector.Resources.Enums;
using CardCollector.Session.Modules;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.InlineQueryHandler.Auction
{
    [SkipCommand]
    public class ShowStickers : InlineQueryHandler
    {
        protected override async Task Execute()
        {
            var stickersList = await Context.Auctions.FindAll(User.Session.GetModule<FiltersModule>(), InlineQuery.Query);
            var offset = int.Parse(InlineQuery.Offset == "" ? "0" : InlineQuery.Offset);
            var newOffset = offset + 50 > stickersList.Count ? "" : (offset + 50).ToString();
            var results = stickersList
                .ToTelegramStickersAsMessage(ChosenInlineResultCommands.select_sticker, offset);
            await MessageController.AnswerInlineQuery(User, InlineQuery.Id, results, newOffset);
        }

        public override bool Match()
        {
            return User.Session.State is UserState.AuctionMenu && InlineQuery.ChatType is ChatType.Sender;
        }
    }
}