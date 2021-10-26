using System;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class CombineStickers : CallbackQueryCommand
    {
        protected override string CommandText => Command.combine_stickers;

        public override async Task Execute()
        {
            var combineModule = User.Session.GetModule<CombineModule>();
            var price = combineModule.CalculateCombinePrice();
            if (User.Cash.Coins < price)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.not_enougth_coins);
            else
            {
                User.Cash.Coins -= price;
                foreach (var (item, count) in combineModule.CombineList)
                    User.Stickers[item.Md5Hash].Count -= count;
                var authors = combineModule.CombineList.Select(i => i.Key.Author).ToList();
                var tier = combineModule.Tier;
                var rnd = new Random();
                var author = authors[rnd.Next(authors.Count)];
                var stickers = await StickerDao.GetListWhere(i => i.Author == author && i.Tier == tier + 1);
                var sticker = stickers[rnd.Next(stickers.Count)];
                await UserStickerRelationDao.AddNew(User, sticker, 1);
                var text = $"{Messages.combined_sticker}:\n" + sticker;
                await MessageController.SendSticker(User, sticker.Id);
                await MessageController.SendMessage(User, text, Keyboard.BackToFilters(sticker.Title));
                User.Session.DeleteModule<CombineModule>();
            }
        }

        public CombineStickers() { }
        public CombineStickers(UserEntity user, Update update) : base(user, update) { }
    }
}