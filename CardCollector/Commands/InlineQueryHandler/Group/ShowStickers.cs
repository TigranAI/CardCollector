using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes.Menu;
using CardCollector.Commands.ChosenInlineResultHandler;
using CardCollector.Controllers;
using CardCollector.Others;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.InlineQueryHandler.Group
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
                .ToTelegramStickers(ChosenInlineResultCommands.chat_send_sticker, offset);
            await MessageController.AnswerInlineQuery(User, InlineQuery.Id, results, newOffset);
        }

        public override bool Match()
        {
            if (InlineQuery.ChatType is not (ChatType.Group or ChatType.Supergroup)) return false;
            return !InlineQuery.Query.StartsWith("#");
        }
    }
}