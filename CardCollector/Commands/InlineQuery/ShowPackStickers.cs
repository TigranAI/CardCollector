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
    public class ShowPackStickers : InlineQueryHandler
    {
        public override async Task Execute()
        {
            var packId = User.Session.GetModule<AdminModule>().SelectedPack.Id;
            var stickers = await StickerDao.GetListWhere(item => item.PackId == packId && item.Contains(Query));
            stickers.Sort(new TierComparer());
            await MessageController.AnswerInlineQuery(InlineQueryId, stickers.ToTelegramResults(Command.select_for_sale_sticker));
        }

        private class TierComparer : IComparer<StickerEntity>
        {
            public int Compare(StickerEntity? x, StickerEntity? y)
            {
                return x?.Tier - y?.Tier ?? 0;
            }
        }

        protected internal override bool Match(UserEntity user, Update update)
        {
            var adminModule = user.Session.GetModule<AdminModule>();
            return user.Session.State == UserState.LoadForSaleSticker &&
                   update.InlineQuery?.ChatType is ChatType.Sender && 
                   adminModule.SelectedPack != null;
        }

        public ShowPackStickers(UserEntity user, Update update) : base(user, update) { }
    }
}