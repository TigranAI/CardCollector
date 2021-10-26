using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class OpenPack : CallbackQueryCommand
    {
        protected override string CommandText => Command.open_pack;

        public override async Task Execute()
        {
            var packId = int.Parse(CallbackData.Split("=")[1]);
            var userPack = await UserPacksDao.GetOne(User.Id, packId);
            if (userPack.Count < 1)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.packs_count_zero, true);
            else
            {
                var packInfo = await PacksDao.GetById(packId);
                packInfo.OpenedCount++;
                userPack.Count--;
                var tier = GetTier(Utilities.rnd.NextDouble() * 100);
                var stickers = await StickerDao.GetListWhere(item =>
                    item.Tier == tier && (packId == 1 || item.PackId == packId));
                var sticker = stickers[Utilities.rnd.Next(stickers.Count)];
                if (User.Stickers.ContainsKey(sticker.Md5Hash))
                    User.Stickers[sticker.Md5Hash].Count++;
                else
                    await UserStickerRelationDao.AddNew(User, sticker, 1);
                await MessageController.SendSticker(User, sticker.Id);
                await MessageController.SendMessage(User, $"{Messages.congratulation}\n{sticker}",
                    userPack.Count > 0
                        ? Keyboard.RepeatCommand(Text.open_more, CallbackData)
                        : Keyboard.BackKeyboard);
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

        public OpenPack() { }
        public OpenPack(UserEntity user, Update update) : base(user, update) { }
    }
}