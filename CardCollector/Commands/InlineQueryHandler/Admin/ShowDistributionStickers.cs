using System.Linq;
using System.Threading.Tasks;
using CardCollector.Commands.ChosenInlineResultHandler;
using CardCollector.Controllers;
using CardCollector.Others;
using CardCollector.Resources.Enums;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.Commands.InlineQueryHandler.Admin
{
    public class ShowDistributionStickers : InlineQueryHandler
    {
        protected override async Task Execute()
        {
            var offset = int.Parse(InlineQuery.Offset == "" ? "0" : InlineQuery.Offset);
            var stickers = await Context.Stickers.ToListAsync();
            var stickersList = stickers
                .Where(item => item.Contains(InlineQuery.Query))
                .ToList();
            var newOffset = offset + 50 > stickersList.Count ? "" : (offset + 50).ToString();
            var results = stickersList
                .ToTelegramStickers(ChosenInlineResultCommands.set_distribution_sticker, offset);
            await MessageController.AnswerInlineQuery(User, InlineQuery.Id, results, newOffset);
        }

        public override bool Match()
        {
            return User.Session.State is UserState.SendDistributionSticker;
        }
    }
}