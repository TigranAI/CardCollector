#nullable enable
using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.InlineQuery
{
    public class ShowStickersInShopPack : InlineQueryCommand
    {
        protected override string CommandText => "";
        
        public override async Task Execute()
        {
            var packId = User.Session.GetModule<ShopModule>().SelectedPack.Id;
            var stickers = await StickerDao.GetListWhere(item => item.PackId == packId && item.Contains(Query));
            stickers.Sort(new TierComparer());
            await MessageController.AnswerInlineQuery(InlineQueryId, stickers.ToTelegramResults(Command.sticker_info));
        }

        private class TierComparer : IComparer<StickerEntity>
        {
            public int Compare(StickerEntity? x, StickerEntity? y)
            {
                return x?.Tier - y?.Tier ?? 0;
            }
        }

        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            var shopModule = user.Session.GetModule<ShopModule>();
            return user.Session.State == UserState.ShopMenu &&
                   update.InlineQuery?.ChatType is ChatType.Sender && 
                   shopModule.SelectedPack != null && 
                   shopModule.SelectedPack.Id != 1;
        }

        public ShowStickersInShopPack() { }
        public ShowStickersInShopPack(UserEntity user, Update update) : base(user, update) { }
    }
}