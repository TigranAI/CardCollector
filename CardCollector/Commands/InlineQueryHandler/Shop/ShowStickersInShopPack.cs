using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Commands.ChosenInlineResultHandler;
using CardCollector.Database.EntityDao;
using CardCollector.Extensions;
using CardCollector.Others;
using CardCollector.Resources.Enums;
using CardCollector.Session.Modules;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.InlineQueryHandler.Shop
{
    [SkipCommand]
    public class ShowStickersInShopPack : InlineQueryHandler
    {
        protected override async Task Execute()
        {
            var offset = int.Parse(InlineQuery.Offset == "" ? "0" : InlineQuery.Offset);
            var length = 0;
            
            var packId = User.Session.GetModule<ShopModule>().SelectedPackId;
            var pack = await Context.Packs.FindById(packId);
            var stickersList = pack.Stickers
                .Where(item => item.Contains(InlineQuery.Query))
                .OrderBy(sticker => sticker.Tier)
                .ToList();
            var results = stickersList
                .And(list => length = list.Count)
                .ToTelegramStickersAsMessage(ChosenInlineResultCommands.sticker_info, offset);
            
            var newOffset = offset + 50 > length ? "" : (offset + 50).ToString();
            await AnswerInlineQuery(User, InlineQuery.Id, results, newOffset);
        }

        public override bool Match()
        {
            if (User.Session.State is not UserState.ShopMenu) return false;
            if (InlineQuery.ChatType is not ChatType.Sender) return false;
            return User.Session.GetModule<ShopModule>().SelectedPackId is not null;
        }
    }
}