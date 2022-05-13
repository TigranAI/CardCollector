using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Commands.ChosenInlineResultHandler;
using CardCollector.Controllers;
using CardCollector.Database.EntityDao;
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
            var packId = User.Session.GetModule<ShopModule>().SelectedPackId;
            var pack = await Context.Packs.FindById(packId);
            var stickersList = pack.Stickers
                .Where(item => item.Contains(InlineQuery.Query))
                .OrderBy(sticker => sticker.Tier)
                .ToList();
            var offset = int.Parse(InlineQuery.Offset == "" ? "0" : InlineQuery.Offset);
            var newOffset = offset + 50 > stickersList.Count ? "" : (offset + 50).ToString();
            var results = stickersList
                .ToTelegramStickersAsMessage(ChosenInlineResultCommands.sticker_info, offset);
            await MessageController.AnswerInlineQuery(User, InlineQuery.Id, results, newOffset);
        }

        public override bool Match()
        {
            if (User.Session.State is not UserState.ShopMenu) return false;
            if (InlineQuery.ChatType is not ChatType.Sender) return false;
            return User.Session.GetModule<ShopModule>().SelectedPackId is not null;
        }
    }
}