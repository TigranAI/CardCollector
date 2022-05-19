using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Commands.ChosenInlineResultHandler;
using CardCollector.Extensions;
using CardCollector.Others;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.InlineQueryHandler.Private
{
    [SkipCommand]
    public class ShowStickersInPrivate : InlineQueryHandler
    {
        protected override async Task Execute()
        {
            var offset = int.Parse(InlineQuery.Offset == "" ? "0" : InlineQuery.Offset);
            var length = 0;
            
            var stickersList = User.Stickers
                .Where(item => item.Count > 0 && item.Sticker.Contains(InlineQuery.Query))
                .OrderByDescending(item => item.LastUsage)
                .ToList();
            var results = stickersList
                .And(list => length = list.Count)
                .ToTelegramStickers(ChosenInlineResultCommands.send_private_sticker, offset);
            
            var newOffset = offset + 50 > length ? "" : (offset + 50).ToString();
            await AnswerInlineQuery(User, InlineQuery.Id, results, newOffset);
        }
        
        public override bool Match()
        {
            return InlineQuery.ChatType is ChatType.Private;
        }
    }
}