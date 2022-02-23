using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes.Menu;
using CardCollector.Commands.ChosenInlineResultHandler;
using CardCollector.Controllers;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.InlineQueryHandler.Shop
{
    [DontAddToCommandStack]
    public class ShowStickersInShopPack : InlineQueryHandler
    {
        protected override async Task Execute()
        {
            var packId = User.Session.GetModule<ShopModule>().SelectedPackId;
            var pack = await Context.Packs.FindById(packId);
            var stickersList = pack.Stickers.Where(item => item.Contains(InlineQuery.Query)).ToList();
            stickersList.Sort(new TierComparer());
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

        public ShowStickersInShopPack(User user, BotDatabaseContext context, InlineQuery inlineQuery) : base(user,
            context, inlineQuery)
        {
        }
    }
}