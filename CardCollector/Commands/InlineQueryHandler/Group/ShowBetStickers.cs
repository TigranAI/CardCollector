using System.Linq;
using System.Threading.Tasks;
using CardCollector.Commands.ChosenInlineResultHandler;
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
            var offset = Offset.Of(InlineQuery);
            var length = 0;
            
            var query = InlineQuery.Query.Substring(CommandText.Length).TrimStart();
            var stickersList = User.Stickers
                .Where(item => item.Count > 0 && item.Sticker.Contains(query) && item.Sticker.Tier != 10)
                .OrderByDescending(item => item.Sticker.Tier)
                .ThenByDescending(item => item.Count)
                .And(list => length = list.Count())
                .ToTelegramMessageResults(ChosenInlineResultCommands.made_a_bet, offset);
            
            await AnswerInlineQuery(User, InlineQuery.Id, stickersList, offset.GetNext(length));
        }

        public override bool Match()
        {
            if (InlineQuery.ChatType is not (ChatType.Group or ChatType.Supergroup)) return false;
            return InlineQuery.Query.StartsWith(CommandText);
        }
    }
}