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
            var offset = int.Parse(InlineQuery.Offset == "" ? "0" : InlineQuery.Offset);
            var length = 0;
            
            var filters = User.Session.GetModule<FiltersModule>();
            var stickersList = User.Stickers
                .Where(item => item.Count > 0 && item.Sticker.Contains(InlineQuery.Query))
                .Select(item => item.Sticker)
                .ToList();
            stickersList = filters.ApplyTo(stickersList);
            var results = stickersList
                .And(list => length = list.Count)
                .ToTelegramStickersAsMessage(ChosenInlineResultCommands.select_sticker, offset);
            
            var newOffset = offset + 50 > length ? "" : (offset + 50).ToString();
            await AnswerInlineQuery(User, InlineQuery.Id, results, newOffset);
        }

        public override bool Match()
        {
            if (User.Session.State is not (UserState.CollectionMenu or UserState.Default)) return false;
            return InlineQuery.ChatType is ChatType.Sender;
        }
    }
}