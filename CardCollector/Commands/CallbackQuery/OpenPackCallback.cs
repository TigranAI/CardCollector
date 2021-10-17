using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class OpenPackCallback : CallbackQuery
    {
        protected override string CommandText => Command.open_pack;
        
        public override async Task Execute()
        {
            var packId = int.Parse(CallbackData.Split("=")[1]);
            var userPack = await UsersPacksDao.GetUserPacks(User.Id);
            var rnd = new Random();
            var packInfo = await PacksDao.GetById(packId);
            var tier = GetTier(rnd.NextDouble() * 100);
            switch (packId)
            {
                case 1 when userPack.RandomCount < 1:
                    await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.packs_count_zero, true);
                    break;
                case 1:
                    userPack.RandomCount--;
                    await OpenPack(await StickerDao.GetListWhere(item => item.Tier == tier));
                    break;
                default:
                    if (!await TryOpen()) 
                        await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.packs_count_zero, true);
                    await OpenPack(await StickerDao.GetListWhere(item => item.Tier == tier && item.Author == packInfo.Author));
                    break;
            }

            async Task OpenPack(List<StickerEntity> stickers)
            {
                packInfo.OpenedCount++;
                await User.ClearChat();
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
            
            async Task<bool> TryOpen()
            {
                var info = await SpecificPackDao.GetInfo(User.Id, packId);
                if (info.Count < 1)
                {
                    if (userPack.AuthorCount < 1) return false;
                    userPack.AuthorCount--;
                    return true;
                }
                info.Count--;
                return true;
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

        public OpenPackCallback() { }
        public OpenPackCallback(UserEntity user, Update update) : base(user, update) { }
    }
}