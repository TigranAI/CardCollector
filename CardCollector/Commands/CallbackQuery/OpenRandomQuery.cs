using System;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class OpenRandomQuery : CallbackQuery
    {
        protected override string CommandText => Command.open_random;

        public override async Task Execute()
        {
            var pack = await UsersPacksDao.GetPackInfo(User.Id, 0);
            if (pack.Count < 1)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.packs_count_zero, true);
            else
            {
                await User.ClearChat();
                pack.Count--;
                var rnd = new Random();
                var tier = GetTier(rnd.NextDouble() * 100);
                var stickers = await StickerDao.GetListWhere(item => item.Tier == tier);
                var sticker = stickers[rnd.Next(stickers.Count)];
                if (User.Stickers.ContainsKey(sticker.Md5Hash))
                {
                    await MessageController.AnswerCallbackQuery(User, CallbackQueryId,
                        $"{Messages.you_collected} {await User.Cash.Payout(User.Stickers)}");
                    User.Stickers[sticker.Md5Hash].Count++;
                }
                else
                    await UserStickerRelationDao.AddNew(User, sticker, 1);

                var stickerMessage = await MessageController.SendSticker(User, sticker.Id);
                var message = await MessageController.SendMessage(User, $"{Messages.congratulation}\n{sticker}");
                User.Session.Messages.Add(stickerMessage.MessageId);
                User.Session.Messages.Add(message.MessageId);
            }
        }

        private int GetTier(double chance)
        {
            return chance switch
            {
                < 0.7 => 4,
                < 3.3 => 3,
                < 16 => 2,
                _ => 1
            };
        }

        public OpenRandomQuery()
        {
        }

        public OpenRandomQuery(UserEntity user, Update update) : base(user, update)
        {
        }
    }
}