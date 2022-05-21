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
            var offset = Offset.Of(InlineQuery);
            var length = 0;
            
            var packId = User.Session.GetModule<ShopModule>().SelectedPackId;
            var pack = await Context.Packs.FindById(packId);
            var results = pack.Stickers
                .Where(item => item.Contains(InlineQuery.Query))
                .OrderBy(sticker => sticker.Tier)
                .And(list => length = list.Count())
                .ToTelegramMessageResults(ChosenInlineResultCommands.sticker_info, offset);
            
            await AnswerInlineQuery(User, InlineQuery.Id, results, offset.GetNext(length));
        }

        public override bool Match()
        {
            if (User.Session.State is not UserState.ShopMenu) return false;
            if (InlineQuery.ChatType is not ChatType.Sender) return false;
            return User.Session.GetModule<ShopModule>().SelectedPackId is not null;
        }
    }
}