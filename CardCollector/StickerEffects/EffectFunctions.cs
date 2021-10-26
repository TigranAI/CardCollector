using System.Threading.Tasks;
using System.Timers;
using CardCollector.Controllers;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;

namespace CardCollector.StickerEffects
{
    public static class EffectFunctions
    {
        public static async void RunAll(object o, ElapsedEventArgs e)
        {
            await GivePacks();
            await GiveTier1();
            await GiveTier2();
        }

        public static async Task GivePacks()
        {
            var users = await UserDao.GetAll();
            foreach (var user in users)
            {
                var stickers = await UserStickerRelationDao.GetListById(user.Id);
                var packsCount = await Random1Pack5Day.GetPacksCount(stickers);
                if (packsCount > 0)
                {
                    var userPacks = await UserPacksDao.GetOne(user.Id, 1);
                    userPacks.Count += packsCount;
                    await MessageController.SendMessage(user,
                        $"{Messages.effect_Random1Pack5Day} {packsCount}{Text.items}");
                }
            }
        }

        public static async Task GiveTier2()
        {
            var users = await UserDao.GetAll();
            foreach (var user in users)
            {
                var stickers = await UserStickerRelationDao.GetListById(user.Id);
                var stickerCount = await RandomSticker2Tier3Day.GetStickersCount(stickers);
                if (stickerCount > 0)
                {
                    var stickerList = await RandomSticker2Tier3Day.GenerateList(stickerCount);
                    var generatedMessage = "";
                    foreach (var (sticker, count) in stickerList)
                    {
                        generatedMessage += $"\n{sticker.Title} {count}{Text.items}";
                        await UserStickerRelationDao.AddNew(user, sticker, count);
                    }
                    await MessageController.SendMessage(user,
                        $"{Messages.effect_RandomSticker2Tier3Day}{generatedMessage}");
                }
            }
        }

        public static async Task GiveTier1()
        {
            var users = await UserDao.GetAll();
            foreach (var user in users)
            {
                var stickers = await UserStickerRelationDao.GetListById(user.Id);
                var stickerCount = await RandomSticker1Tier3Day.GetStickersCount(stickers);
                if (stickerCount > 0)
                {
                    var stickerList = await RandomSticker1Tier3Day.GenerateList(stickerCount);
                    var generatedMessage = "";
                    foreach (var (sticker, count) in stickerList)
                    {
                        generatedMessage += $"\n{sticker.Title} {count}{Text.items}";
                        await UserStickerRelationDao.AddNew(user, sticker, count);
                    }
                    await MessageController.SendMessage(user,
                        $"{Messages.effect_RandomSticker1Tier3Day}{generatedMessage}");
                }
            }
        }
    }
}