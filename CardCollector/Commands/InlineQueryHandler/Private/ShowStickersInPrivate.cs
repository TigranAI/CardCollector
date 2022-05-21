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
            var offset = Offset.Of(InlineQuery);
            var length = 0;
            
            var results = User.Stickers
                .Where(item => item.Count > 0 && item.Sticker.Contains(InlineQuery.Query))
                .OrderByDescending(item => item.LastUsage)
                .And(list => length = list.Count())
                .ToTelegramResults(ChosenInlineResultCommands.send_private_sticker, offset);
            
            await AnswerInlineQuery(User, InlineQuery.Id, results, offset.GetNext(length));
        }
        
        public override bool Match()
        {
            return InlineQuery.ChatType is ChatType.Private;
        }
    }
}