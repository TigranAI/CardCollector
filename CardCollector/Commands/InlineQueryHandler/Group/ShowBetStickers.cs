using System.Linq;
using System.Threading.Tasks;
using CardCollector.Extensions;
using CardCollector.Others;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.InlineQueryHandler.Group
{
    public class ShowBetStickers : InlineQueryHandler
    {
        protected override string CommandText => InlineQueryCommands.roulette;

        protected override async Task Execute()
        {
            var offset = int.Parse(InlineQuery.Offset == "" ? "0" : InlineQuery.Offset);
            var length = 0;
            var query = InlineQuery.Query.Substring(CommandText.Length).TrimStart();
            var stickersList = User.Stickers
                .Where(item => item.Count > 0 && item.Sticker.Contains(query) && item.Sticker.Tier != 10)
                .OrderByDescending(item => item.Sticker.Tier)
                .ThenByDescending(item => item.Count)
                .And(list => length = list.Count())
                .Skip(offset)
                .Take(50)
                .Select(sticker => sticker.AsTelegramBetArticle());
            var newOffset = offset + 50 > length ? "" : (offset + 50).ToString();
            await AnswerInlineQuery(User, InlineQuery.Id, stickersList, newOffset);
        }

        public override bool Match()
        {
            if (InlineQuery.ChatType is not (ChatType.Group or ChatType.Supergroup)) return false;
            return InlineQuery.Query.StartsWith(CommandText);
        }
    }
}