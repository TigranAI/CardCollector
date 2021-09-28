using System;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class CombineStickers : CallbackQuery
    {
        protected override string CommandText => Command.combine_stickers;
        public override async Task Execute()
        {
            if (User.Cash.Coins < User.Session.CombineCoinsPrice)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.not_enougth_coins);
            else if (User.Cash.Gems < User.Session.CombineGemsPrice)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.not_enougth_gems);
            else
            {
                await User.ClearChat();
                User.Cash.Coins -= User.Session.CombineCoinsPrice;
                User.Cash.Gems -= User.Session.CombineGemsPrice;
                foreach (var item in User.Session.CombineList.Values)
                {
                    await User.Session.PayOutOne(item.Md5Hash);
                    User.Stickers[item.Md5Hash].Count -= item.Count;
                }
                var authors = User.Session.CombineList.Values.Select(i => i.Author).ToList();
                var tier = User.Session.CombineList.Values.First().Tier;
                var rnd = new Random();
                var author = authors[rnd.Next(authors.Count)];
                var stickers = await StickerDao.GetListWhere(i => i.Author == author && i.Tier == tier + 1);
                var sticker = stickers[rnd.Next(stickers.Count)];
                if (User.Stickers.ContainsKey(sticker.Md5Hash))
                    await User.Session.PayOutOne(sticker.Md5Hash);
                await UserStickerRelationDao.AddNew(User, sticker, 1);
                var text = $"{Messages.combined_sticker}:\n" + sticker;
                var stickerMessage = await MessageController.SendSticker(User, sticker.Id);
                var message = await MessageController.SendMessage(User, text, Keyboard.BackToFilters(sticker.Title));
                User.Session.Messages.Add(stickerMessage.MessageId);
                User.Session.Messages.Add(message.MessageId);
            }
        }

        public CombineStickers() { }
        public CombineStickers(UserEntity user, Update update) : base(user, update) { }
    }
}