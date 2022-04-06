using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes.Menu;
using CardCollector.Commands.CallbackQueryHandler;
using CardCollector.Controllers;
using CardCollector.Others;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.InlineQueryHandler.Channel
{
    [DontAddToCommandStack]
    public class ShowStickers : InlineQueryHandler
    {
        protected override async Task Execute()
        {
            var stickersList = User.Stickers
                .Where(item => item.Count > 0 && item.Sticker.Contains(InlineQuery.Query))
                .OrderByDescending(item => item.LastUsage)
                .Select(item => item.Sticker)
                .ToList();
            var offset = int.Parse(InlineQuery.Offset == "" ? "0" : InlineQuery.Offset);
            var newOffset = offset + 50 > stickersList.Count() ? "" : (offset + 50).ToString();
            var results = stickersList
                .ToTelegramStickers(CallbackQueryCommands.ignore, offset);
            await MessageController.AnswerInlineQuery(User, InlineQuery.Id, results, newOffset);
        }

        public override bool Match()
        {
            return InlineQuery.ChatType is ChatType.Channel;
        }
    }
}