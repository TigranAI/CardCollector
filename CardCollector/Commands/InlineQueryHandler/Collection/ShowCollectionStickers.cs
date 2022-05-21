using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Commands.ChosenInlineResultHandler;
using CardCollector.Extensions;
using CardCollector.Others;
using CardCollector.Resources.Enums;
using CardCollector.Session.Modules;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.InlineQueryHandler.Collection
{
    [SkipCommand]
    public class ShowCollectionStickers : InlineQueryHandler
    {
        protected override async Task Execute()
        {
            var offset = Offset.Of(InlineQuery);
            var length = 0;
            
            var results = User.Stickers
                .Where(item => item.Count > 0 && item.Sticker.Contains(InlineQuery.Query))
                .ApplyFilters(User.Session.GetModule<FiltersModule>())
                .And(list => length = list.Count())
                .ToTelegramResults(ChosenInlineResultCommands.select_sticker, offset);
            
            await AnswerInlineQuery(User, InlineQuery.Id, results, offset.GetNext(length));
        }

        public override bool Match()
        {
            if (User.Session.State is not (UserState.CollectionMenu or UserState.Default)) return false;
            return InlineQuery.ChatType is ChatType.Sender;
        }
    }
}