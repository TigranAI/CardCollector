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
    public class ShowCombineStickers : InlineQueryHandler
    {
        protected override async Task Execute()
        {
            var offset = int.Parse(InlineQuery.Offset == "" ? "0" : InlineQuery.Offset);
            var length = 0;

            var module = User.Session.GetModule<CombineModule>();
            var sticker = module.CombineList.FirstOrDefault()?.Item1;
            var stickersList = User.Stickers
                .Where(item => item.Count > 0
                               && item.Sticker.Contains(InlineQuery.Query)
                               && !module.CombineList.Any(pair => item.Sticker.Id == pair.Item1.Id)
                               && (sticker == null || sticker.Tier == item.Sticker.Tier))
                .Select(item => item.Sticker)
                .ToList();
            var results = stickersList
                .And(list => length = list.Count)
                .ToTelegramStickersAsMessage(ChosenInlineResultCommands.select_sticker, offset);

            var newOffset = offset + 50 > length ? "" : (offset + 50).ToString();
            await AnswerInlineQuery(User, InlineQuery.Id, results, newOffset);
        }

        public override bool Match()
        {
            return User.Session.State == UserState.CombineMenu && InlineQuery.ChatType is ChatType.Sender;
        }
    }
}