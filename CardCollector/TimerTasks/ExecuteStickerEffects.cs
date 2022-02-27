using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using CardCollector.Database;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.StickerEffects;
using Microsoft.EntityFrameworkCore;

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
            using (var context = new BotDatabaseContext())
            {
                var users = await context.Users.Where(user => !user.IsBlocked).ToListAsync();
                foreach (var user in users)
                {
                    var packsCount = user.Stickers
                        .Where(item =>
                        {
                            var result = item.Sticker.Effect == Effect.Random1Pack5Day
                                         && item.GivePrizeDate.CompareTo(DateTime.Today.AddDays(-5)) <= 0;
                            if (result) item.GivePrizeDate = DateTime.Today;
                            return result;
                        })
                        .Sum(sticker => sticker.Count);
                    if (packsCount > 0)
                    {
                        var pack = await context.Packs.FindById(1);
                        user.AddPack(pack, packsCount);

                        if (user.Settings[Resources.Enums.UserSettings.StickerEffects])
                            await user.Messages.SendMessage(user,
                                $"{Messages.effect_Random1Pack5Day} {packsCount}{Text.items}");
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        public static async Task GiveTier2()
        {
            using (var context = new BotDatabaseContext())
            {
                var users = await context.Users.Where(user => !user.IsBlocked).ToListAsync();
                foreach (var user in users)
                {
                    var stickersCount = user.Stickers
                        .Where(item =>
                        {
                            var result = item.Sticker.Effect == Effect.RandomSticker2Tier3Day
                                         && item.GivePrizeDate.CompareTo(DateTime.Today.AddDays(-3)) <= 0;
                            if (result) item.GivePrizeDate = DateTime.Today;
                            return result;
                        })
                        .Sum(sticker => sticker.Count);
                    if (stickersCount > 0)
                    {
                        var message = Messages.effect_RandomSticker2Tier3Day;
                        var prizeList = await GenerateList(context, stickersCount, 2);
                        foreach (var sticker in prizeList.DistinctBy(item => item.Id))
                        {
                            var count = prizeList.Count(sticker1 => sticker1.Id == sticker.Id);
                            message += $"\n{sticker.Title} {Text.by} {sticker.Author} {count}{Text.items}";
                            await user.AddSticker(context, sticker, count);
                        }

                        if (user.Settings[Resources.Enums.UserSettings.StickerEffects])
                            await user.Messages.SendMessage(user, message);
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        public static async Task GiveTier1()
        {
            using (var context = new BotDatabaseContext())
            {
                var users = await context.Users.Where(user => !user.IsBlocked).ToListAsync();
                foreach (var user in users)
                {
                    var stickersCount = user.Stickers
                        .Where(item =>
                        {
                            var result = item.Sticker.Effect == Effect.RandomSticker1Tier2Day
                                         && item.GivePrizeDate.CompareTo(DateTime.Today.AddDays(-2)) <= 0;
                            if (result) item.GivePrizeDate = DateTime.Today;
                            return result;
                        })
                        .Sum(sticker => sticker.Count);
                    if (stickersCount > 0)
                    {
                        var message = Messages.effect_RandomSticker1Tier2Day;
                        var prizeList = await GenerateList(context, stickersCount, 1);
                        foreach (var sticker in prizeList.DistinctBy(item => item.Id))
                        {
                            var count = prizeList.Count(sticker1 => sticker1.Id == sticker.Id);
                            message += $"\n{sticker.Title} {Text.by} {sticker.Author} {count}{Text.items}";
                            await user.AddSticker(context, sticker, count);
                        }

                        if (user.Settings[Resources.Enums.UserSettings.StickerEffects])
                            await user.Messages.SendMessage(user, message);
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        private static async Task<List<Sticker>> GenerateList(BotDatabaseContext context, int stickersCount, int tier)
        {
            var stickersByTier = await context.Stickers.FindAllByTier(tier);
            var result = new List<Sticker>();
            while (result.Count != stickersCount)
            {
                result.Add(stickersByTier.Random());
            }

            return result;
        }
    }
}