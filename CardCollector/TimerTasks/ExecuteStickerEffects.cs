using System;
using System.Threading.Tasks;
using System.Timers;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.StickerEffects;

namespace CardCollector.TimerTasks
{
    public class ExecuteStickerEffects : TimerTask
    {
        protected override TimeSpan RunAt => Constants.DEBUG 
            ? new TimeSpan(DateTime.Now.TimeOfDay.Hours,
                DateTime.Now.TimeOfDay.Minutes + Constants.TEST_ALERTS_INTERVAL, 0) 
            : new TimeSpan(11, 0, 0);
        
        protected override async void TimerCallback(object o, ElapsedEventArgs e)
        {
            await GivePacks();
            await GiveTier1();
            await GiveTier2();
        }
        
        public async Task GivePacks()
        {
            var users = await UserDao.GetAllWhere(user => !user.IsBlocked);
            var settings = await SettingsDao.GetAll();
            foreach (var user in users)
            {
                var stickers = await UserStickerRelationDao.GetListById(user.Id);
                var packsCount = await Random1Pack5Day.GetPacksCount(stickers);
                if (packsCount > 0)
                {
                    var userPacks = await UserPacksDao.GetOne(user.Id, 1);
                    userPacks.Count += packsCount;
                    try {
                        if (settings[user.Id][UserSettingsEnum.StickerEffects])
                            await MessageController.SendMessage(user,
                                $"{Messages.effect_Random1Pack5Day} {packsCount}{Text.items}", addToList: false);
                    }
                    catch {
                        await MessageController.SendMessage(user,
                            $"{Messages.effect_Random1Pack5Day} {packsCount}{Text.items}", addToList: false);
                    }
                }
            }
        }

        public static async Task GiveTier2()
        {
            var users = await UserDao.GetAll();
            var settings = await SettingsDao.GetAll();
            foreach (var user in users)
            {
                user.Stickers = await UserStickerRelationDao.GetListById(user.Id);
                var stickerCount = await RandomSticker2Tier3Day.GetStickersCount(user.Stickers);
                if (stickerCount > 0)
                {
                    var stickerList = await RandomSticker2Tier3Day.GenerateList(stickerCount);
                    var generatedMessage = "";
                    foreach (var (sticker, count) in stickerList)
                    {
                        generatedMessage += $"\n{sticker.Title} {count}{Text.items}";
                        await UserStickerRelationDao.AddSticker(user, sticker, count);
                    }
                    try {
                        if (settings[user.Id][UserSettingsEnum.StickerEffects])
                            await MessageController.SendMessage(user,
                                $"{Messages.effect_RandomSticker2Tier3Day}{generatedMessage}", addToList: false);
                    }
                    catch {
                        await MessageController.SendMessage(user,
                            $"{Messages.effect_RandomSticker2Tier3Day}{generatedMessage}", addToList: false);
                    }
                }
            }
        }

        public static async Task GiveTier1()
        {
            var users = await UserDao.GetAll();
            var settings = await SettingsDao.GetAll();
            foreach (var user in users)
            {
                user.Stickers = await UserStickerRelationDao.GetListById(user.Id);
                var stickerCount = await RandomSticker1Tier3Day.GetStickersCount(user.Stickers);
                if (stickerCount > 0)
                {
                    var stickerList = await RandomSticker1Tier3Day.GenerateList(stickerCount);
                    var generatedMessage = "";
                    foreach (var (sticker, count) in stickerList)
                    {
                        generatedMessage += $"\n{sticker.Title} {count}{Text.items}";
                        await UserStickerRelationDao.AddSticker(user, sticker, count);
                    }
                    try {
                        if (settings[user.Id][UserSettingsEnum.StickerEffects])
                            await MessageController.SendMessage(user,
                                $"{Messages.effect_RandomSticker1Tier3Day}{generatedMessage}", addToList: false);
                    }
                    catch {
                        await MessageController.SendMessage(user,
                            $"{Messages.effect_RandomSticker1Tier3Day}{generatedMessage}", addToList: false);
                    }
                }
            }
        }
    }
}