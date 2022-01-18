using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.Entity.NotMapped;
using CardCollector.DataBase.EntityDao;
using CardCollector.Migrations.Database_old;
using CardCollector.Migrations.Database_old.Entity;
using CardCollector.Resources;
using CardCollector.StickerEffects;
using CardCollector.UserDailyTask;
using Microsoft.EntityFrameworkCore;
using CountLogs = CardCollector.DataBase.Entity.CountLogs;
using Level = CardCollector.DataBase.Entity.Level;
using Payment = CardCollector.DataBase.Entity.Payment;
using UserLevel = CardCollector.DataBase.Entity.UserLevel;
using UserMessages = CardCollector.DataBase.Entity.UserMessages;
using UserPacks = CardCollector.DataBase.Entity.UserPacks;
using UserSettings = CardCollector.DataBase.Entity.UserSettings;

namespace CardCollector.Migrations
{
    public static class DatabaseMigration
    {
        public static async Task Migrate(BotDatabaseContext context)
        {
            await InitPacks(context);
            await InitLevels(context);
            await context.SaveChangesAsync();
            using (var oldContext = new BotDatabase())
            {
                await ImportStickerPacks(context, oldContext);
                await context.SaveChangesAsync();
                await ImportSpecialOrders(context, oldContext);
                await context.SaveChangesAsync();
                await ImportLevels(context, oldContext);
                await context.SaveChangesAsync();
                await ImportUsers(context, oldContext);
                await context.SaveChangesAsync();
                await ImportAuctions(context, oldContext);
                await context.SaveChangesAsync();
                await ImportCountLogs(context, oldContext);
                await context.SaveChangesAsync();
                await ImportPayments(context, oldContext);
                await context.SaveChangesAsync();
            }
        }

        private static async Task ImportPayments(BotDatabaseContext context, BotDatabase oldContext)
        {
            var oldPayments = await oldContext.Payments.ToListAsync();
            if (oldPayments.Count == 0) return;
            var result = new LinkedList<Payment>();
            foreach (var oldPayment in oldPayments)
            {
                var user = await context.Users.FindByChatId(oldPayment.UserId);
                var payment = FromOldPayment(user, oldPayment);
                result.AddLast(payment);
            }

            await context.Payments.AddRangeAsync(result);
        }

        private static Payment FromOldPayment(User user, Database_old.Entity.Payment oldPayment)
        {
            var payment = new Payment()
            {
                User = user,
                TotalAmount = oldPayment.TotalAmount,
                InvoicePayload = oldPayment.InvoicePayload,
                ProviderPaymentChargeId = oldPayment.ProviderPaymentChargeId,
                TelegramPaymentChargeId = oldPayment.TelegramPaymentChargeId,
            };
            return payment;
        }

        private static async Task ImportCountLogs(BotDatabaseContext context, BotDatabase oldContext)
        {
            var oldCountLogs = await oldContext.CountLogs.ToListAsync();
            if (oldCountLogs.Count == 0) return;
            var result = new LinkedList<CountLogs>();
            foreach (var oldCountLog in oldCountLogs)
            {
                var countLog = FromOldCountLog(oldCountLog);
                result.AddLast(countLog);
            }

            await context.CountLogs.AddRangeAsync(result);
        }

        private static CountLogs FromOldCountLog(Database_old.Entity.CountLogs oldCountLog)
        {
            var countLog = new CountLogs()
            {
                Date = oldCountLog.Date,
                PeopleDonated = oldCountLog.PeopleDonated,
                PeopleCompletedDailyTask = oldCountLog.PeopleCompletedDailyTask,
                PeoplePutsStickerToAuction = oldCountLog.PeoplePutsStickerToAuction,
                PeopleCollectedIncomeMoreTimes = oldCountLog.PeopleCollectedIncomeMoreTimes,
                PeopleSendsStickerOneOrMoreTimes = oldCountLog.PeopleSendsStickerOneOrMoreTimes,
                PeopleCollectedIncomeOneToThreeTimes = oldCountLog.PeopleCollectedIncomeOneToThreeTimes
            };
            return countLog;
        }

        private static async Task ImportAuctions(BotDatabaseContext context, BotDatabase oldContext)
        {
            var oldAuctions = await oldContext.Auction
                .Where(item => item.Count > 0)
                .ToListAsync();
            if (oldAuctions.Count == 0) return;
            var results = new LinkedList<Auction>();
            foreach (var oldAuction in oldAuctions)
            {
                var sticker = await context.Stickers.FindByFileId(oldAuction.StickerId);
                var user = await context.Users.FindByChatId(oldAuction.Trader);
                var auction = FromOldAuction(user, sticker, oldAuction);
                results.AddLast(auction);
            }

            await context.Auctions.AddRangeAsync(results);
        }

        private static Auction FromOldAuction(User user, Sticker sticker, AuctionEntity oldAuction)
        {
            var auction = new Auction()
            {
                Trader = user,
                Sticker = sticker,
                Price = oldAuction.Price,
                Count = oldAuction.Count
            };
            return auction;
        }

        private static async Task ImportLevels(BotDatabaseContext context, BotDatabase oldContext)
        {
            var oldLevels = await oldContext.Levels
                .Where(item => item.LevelValue > 10)
                .ToListAsync();
            if (oldLevels.Count == 0) return;
            var result = new LinkedList<Level>();
            foreach (var oldLevel in oldLevels)
            {
                var level = FromOldLevel(oldLevel);
                result.AddLast(level);
            }

            await context.Levels.AddRangeAsync(result);
        }

        private static Level FromOldLevel(Database_old.Entity.Level oldLevel)
        {
            var level = new Level()
            {
                LevelExpGoal = oldLevel.LevelExpGoal,
                LevelValue = oldLevel.LevelValue,
            };
            if (oldLevel.JSONLevelReward != null)
                level.LevelReward = Utilities.FromJson<LevelReward>(oldLevel.JSONLevelReward);
            return level;
        }

        private static async Task ImportSpecialOrders(BotDatabaseContext context, BotDatabase oldContext)
        {
            var specialOrders = new LinkedList<SpecialOrder>();
            var specialOrdersOld = await oldContext.Shop.ToListAsync();
            foreach (var specialOrderOld in specialOrdersOld)
            {
                var pack = await context.Packs.FindById(specialOrderOld.PackId);
                var specialOrder = FromOldSpecialOrder(specialOrderOld, pack);
                specialOrders.AddLast(specialOrder);
            }

            await context.SpecialOrders.AddRangeAsync(specialOrders);
        }

        private static SpecialOrder FromOldSpecialOrder(ShopEntity specialOrderOld, Pack pack)
        {
            var specialOrder = new SpecialOrder()
            {
                Id = specialOrderOld.Id,
                Pack = pack,
                Title = specialOrderOld.Title,
                IsInfinite = specialOrderOld.IsInfinite,
                Count = specialOrderOld.Count,
                PriceCoins = specialOrderOld.PriceCoins,
                PriceGems = specialOrderOld.PriceGems,
                Discount = specialOrderOld.Discount,
                TimeLimited = specialOrderOld.TimeLimited,
                TimeLimit = specialOrderOld.TimeLimit
            };
            if (specialOrderOld.Description != null) specialOrder.Description = specialOrderOld.Description;
            if (specialOrderOld.ImageId != null) specialOrder.PreviewFileId = specialOrderOld.ImageId;
            return specialOrder;
        }

        private static async Task ImportUsers(BotDatabaseContext context, BotDatabase oldContext)
        {
            var users = new LinkedList<User>();
            var usersOld = await oldContext.Users.ToListAsync();
            foreach (var userOld in usersOld)
            {
                var user = FromOldUser(userOld);
                user.Level = await FindLevel(oldContext, userOld.Id);
                user.Cash = await FindCash(oldContext, userOld.Id);
                user.Settings = await FindUserSettings(oldContext, userOld.Id);
                user.Messages = await FindUserMessages(oldContext, userOld.Id);
                user.Stickers = await FindUserStickers(user, context, oldContext, userOld.Id);
                user.Packs = await FindUserPacks(user, context, oldContext, userOld.Id);
                user.DailyTasks = await FindUserDailyTasks(user, oldContext, userOld.Id);
                user.SpecialOrdersUser = await FindSpecialOrdersUser(user, context, oldContext, userOld.Id);
                users.AddLast(user);
            }

            await context.Users.AddRangeAsync(users);
        }

        private static async Task<ICollection<SpecialOrderUser>> FindSpecialOrdersUser(User user,
            BotDatabaseContext context, BotDatabase oldContext, long userOldId)
        {
            var result = new LinkedList<SpecialOrderUser>();
            var oldSpecialUserOrders = await oldContext.SpecialOfferUsers
                .Where(item => item.UserId == userOldId)
                .ToListAsync();
            if (oldSpecialUserOrders.Count == 0) return result;
            foreach (var oldSpecialUserOrder in oldSpecialUserOrders)
            {
                var specialOrder = await context.SpecialOrders
                    .SingleOrDefaultAsync(item => item.Id == oldSpecialUserOrder.OfferId);
                if (specialOrder == null) continue;
                var specialOrderUser = FromOldSpecialOrderUser(user, specialOrder);
                result.AddLast(specialOrderUser);
            }

            return result;
        }

        private static SpecialOrderUser FromOldSpecialOrderUser(User user, SpecialOrder specialOrder)
        {
            var specialOrderUser = new SpecialOrderUser()
            {
                User = user,
                Order = specialOrder,
                Timestamp = DateTime.Now
            };
            return specialOrderUser;
        }

        private static async Task<ICollection<DailyTask>> FindUserDailyTasks(User user, BotDatabase oldContext,
            long userOldId)
        {
            var result = new LinkedList<DailyTask>();
            var oldUserTasks = await oldContext.DailyTasks
                .Where(item => item.UserId == userOldId)
                .ToListAsync();
            if (oldUserTasks.Count == 0)
            {
                result.AddLast(new DailyTask()
                {
                    User = user,
                    TaskId = TaskKeys.SendStickersToUsers,
                    Progress = TaskGoals.Goals[TaskKeys.SendStickersToUsers]
                });
                return result;
            }

            foreach (var oldUserTask in oldUserTasks)
            {
                var userTask = FromOldDailyTask(user, oldUserTask);
                result.AddLast(userTask);
            }

            return result;
        }

        private static DailyTask FromOldDailyTask(User user, DailyTaskEntity oldUserTask)
        {
            var userTask = new DailyTask()
            {
                User = user,
                TaskId = (TaskKeys) oldUserTask.TaskId,
                Progress = TaskGoals.Goals[(TaskKeys) oldUserTask.TaskId]
            };
            return userTask;
        }

        private static async Task<ICollection<UserPacks>> FindUserPacks(User user, BotDatabaseContext context,
            BotDatabase oldContext, long userOldId)
        {
            var result = new LinkedList<UserPacks>();
            var oldUserPacks = await oldContext.UsersPacks
                .Where(packs => packs.UserId == userOldId)
                .ToListAsync();
            if (oldUserPacks.Count == 0) return result;
            foreach (var oldUserPack in oldUserPacks)
            {
                var pack = await context.Packs.FindById(oldUserPack.PackId);
                var userPack = FromOldUserPack(user, pack, oldUserPack);
                result.AddLast(userPack);
            }

            return result;
        }

        private static UserPacks FromOldUserPack(User user, Pack pack, Database_old.Entity.UserPacks oldUserPack)
        {
            var userPack = new UserPacks()
            {
                User = user,
                Pack = pack,
                Count = oldUserPack.Count
            };
            return userPack;
        }

        private static async Task<ICollection<UserSticker>> FindUserStickers(User user, BotDatabaseContext context,
            BotDatabase oldContext, long userOldId)
        {
            var result = new LinkedList<UserSticker>();
            var oldStickers = await oldContext.UserStickerRelations
                .Where(item => item.UserId == userOldId)
                .ToListAsync();
            if (oldStickers.Count == 0) return result;
            foreach (var oldUserSticker in oldStickers)
            {
                var sticker = await context.Stickers.FindByFileId(oldUserSticker.StickerId);
                var userSticker = FromOldUserSticker(user, sticker, oldUserSticker);
                result.AddLast(userSticker);
            }

            return result;
        }

        private static UserSticker FromOldUserSticker(User user, Sticker sticker,
            UserStickerRelation oldUserSticker)
        {
            var userSticker = new UserSticker()
            {
                User = user,
                Count = oldUserSticker.Count,
                Payout = oldUserSticker.Payout
            };
            if (oldUserSticker.AdditionalData != "" && oldUserSticker.AdditionalData != null)
                userSticker.GivePrizeDate = DateTime.Parse(oldUserSticker.AdditionalData, Constants.TimeCulture);
            userSticker.Sticker = sticker;
            return userSticker;
        }

        private static async Task<UserMessages> FindUserMessages(BotDatabase oldContext, long userOldId)
        {
            var result = new UserMessages();
            var messagesOld = await oldContext.UserMessages.SingleOrDefaultAsync(item => item.UserId == userOldId);
            if (messagesOld == null) return result;
            result.MenuMessageId = messagesOld.MenuId;
            result.CollectIncomeMessageId = messagesOld.CollectIncomeId;
            result.TopUsersMessageId = messagesOld.TopUsersId;
            result.DailyTaskAlertMessageId = messagesOld.DailyTaskProgressId;
            result.DailyTaskProgressMessageId = messagesOld.DailyTaskId;
            return result;
        }

        private static async Task<UserSettings> FindUserSettings(BotDatabase oldContext, long userOldId)
        {
            var result = new UserSettings();
            var oldSettings = await oldContext.Settings.SingleOrDefaultAsync(item => item.UserId == userOldId);
            if (oldSettings == null) return result;
            result.Settings = oldSettings.settings;
            return result;
        }

        private static async Task<Cash> FindCash(BotDatabase oldContext, long userOldId)
        {
            var result = new Cash();
            var oldCash = await oldContext.Cash.SingleOrDefaultAsync(item => item.UserId == userOldId);
            if (oldCash == null) return result;
            result.Coins = oldCash.Coins;
            result.Gems = oldCash.Gems;
            result.MaxCapacity = oldCash.MaxCapacity;
            return result;
        }

        private static async Task<UserLevel> FindLevel(BotDatabase oldContext, long userOldId)
        {
            var result = new UserLevel();
            var oldUserLevel = await oldContext.UserLevel.SingleOrDefaultAsync(item => item.UserId == userOldId);
            if (oldUserLevel == null) return result;
            result.Level = oldUserLevel.Level;
            result.CurrentExp = oldUserLevel.CurrentExp;
            result.TotalExp = oldUserLevel.TotalExp;
            return result;
        }

        private static User FromOldUser(UserEntity userOld)
        {
            var user = new User()
            {
                ChatId = userOld.Id,
                Username = userOld.Username,
                IsBlocked = userOld.IsBlocked,
                PrivilegeLevel = (PrivilegeLevel) userOld._privilegeLevel,
                FirstReward = userOld.FirstReward
            };
            return user;
        }

        private static async Task ImportStickerPacks(BotDatabaseContext context, BotDatabase oldContext)
        {
            var randomPack = await context.Packs.SingleAsync(pack => pack.Id == 1);
            var oldRandomPack = await oldContext.Packs.SingleAsync(pack => pack.Id == 1);
            randomPack.OpenedCount = oldRandomPack.OpenedCount;

            var packList = new LinkedList<Pack>();
            var packsOld = await oldContext.Packs.Where(pack => pack.Id != 1).ToListAsync();
            foreach (var packOld in packsOld)
            {
                var pack = FromOldPack(packOld);
                pack.Stickers = await FindStickers(pack, oldContext, packOld.Id);
                packList.AddLast(pack);
            }

            await context.AddRangeAsync(packList);
        }

        private static async Task<ICollection<Sticker>> FindStickers(Pack pack, BotDatabase oldContext,
            int packOldId)
        {
            var results = new LinkedList<Sticker>();
            var stickersOld = await oldContext.Stickers
                .Where(entity => entity.PackId == packOldId)
                .ToListAsync();
            foreach (var stickerOld in stickersOld)
            {
                var sticker = FromOldSticker(pack, stickerOld);
                results.AddLast(sticker);
            }
            return results;
        }

        private static Sticker FromOldSticker(Pack pack, StickerEntity stickerOld)
        {
            var sticker = new Sticker()
            {
                Pack = pack,
                Title = stickerOld.Title,
                Author = stickerOld.Author,
                Income = stickerOld.Income,
                IncomeTime = stickerOld.IncomeTime,
                Tier = stickerOld.Tier,
                Effect = (Effect) stickerOld.Effect,
                Emoji = stickerOld.Emoji,
                FileId = stickerOld.Id,
                IsAnimated = stickerOld.Animated
            };
            if (stickerOld.ForSaleId != null)
            {
                sticker.ForSaleFileId = stickerOld.ForSaleId;
                sticker.IsForSaleAnimated = stickerOld.Animated;
            }

            if (stickerOld.Description != null) sticker.Description = stickerOld.Description;
            return sticker;
        }

        private static Pack FromOldPack(PackEntity packOld)
        {
            var pack = new Pack()
            {
                Id = packOld.Id,
                PriceGems = 100,
                PriceCoins = -1,
                Author = packOld.Author,
                OpenedCount = packOld.OpenedCount,
            };
            if (packOld.Description != null) pack.Description = packOld.Description;
            if (packOld.StickerPreview != null)
            {
                pack.PreviewFileId = packOld.StickerPreview;
                pack.IsPreviewAnimated = packOld.Animated;
            }

            return pack;
        }

        private static async Task InitPacks(BotDatabaseContext context)
        {
            await context.Packs.AddAsync(new Pack()
            {
                Id = 1,
                Author = Text.random_author,
                Description = Text.random_author_description,
                PriceCoins = 1000,
                PriceGems = 100,
                PreviewFileId = "CAACAgIAAxkBAAIWs2DuY4vB50ARmyRwsgABs_7o5weDaAAC-g4AAmq4cUtH6M1FoN4bxSAE",
                IsPreviewAnimated = false,
            });
        }

        private static async Task InitLevels(BotDatabaseContext context)
        {
            await CreateLevel0(context);
            await CreateLevel1(context);
            await CreateLevel2(context);
            await CreateLevel3(context);
            await CreateLevel4(context);
            await CreateLevel5(context);
            await CreateLevel6(context);
            await CreateLevel7(context);
            await CreateLevel8(context);
            await CreateLevel9(context);
            await CreateLevel10(context);
        }

        private static async Task CreateLevel10(BotDatabaseContext context)
        {
            await context.Levels.AddAsync(new Level()
            {
                Id = 11, LevelValue = 10, LevelExpGoal = 665052, LevelReward = new LevelReward()
                {
                    CashCapacity = 50, RandomPacks = 3, RandomStickerTier = 4
                }
            });
        }

        private static async Task CreateLevel9(BotDatabaseContext context)
        {
            await context.Levels.AddAsync(new Level()
            {
                Id = 10, LevelValue = 9, LevelExpGoal = 262737, LevelReward = new LevelReward()
                {
                    CashCapacity = 50, RandomPacks = 3
                }
            });
        }

        private static async Task CreateLevel8(BotDatabaseContext context)
        {
            await context.Levels.AddAsync(new Level()
            {
                Id = 9, LevelValue = 8, LevelExpGoal = 102176, LevelReward = new LevelReward()
                {
                    CashCapacity = 50, RandomPacks = 3
                }
            });
        }

        private static async Task CreateLevel7(BotDatabaseContext context)
        {
            await context.Levels.AddAsync(new Level()
            {
                Id = 8, LevelValue = 7, LevelExpGoal = 38924, LevelReward = new LevelReward()
                {
                    CashCapacity = 50, RandomPacks = 3
                }
            });
        }

        private static async Task CreateLevel6(BotDatabaseContext context)
        {
            await context.Levels.AddAsync(new Level()
            {
                Id = 7, LevelValue = 6, LevelExpGoal = 7209, LevelReward = new LevelReward()
                {
                    CashCapacity = 50, RandomPacks = 3, RandomStickerTier = 3
                }
            });
        }

        private static async Task CreateLevel5(BotDatabaseContext context)
        {
            await context.Levels.AddAsync(new Level()
            {
                Id = 6, LevelValue = 5, LevelExpGoal = 2563, LevelReward = new LevelReward()
                {
                    CashCapacity = 50, RandomPacks = 3
                }
            });
        }

        private static async Task CreateLevel4(BotDatabaseContext context)
        {
            await context.Levels.AddAsync(new Level()
            {
                Id = 5, LevelValue = 4, LevelExpGoal = 855, LevelReward = new LevelReward()
                {
                    CashCapacity = 50, RandomPacks = 4
                }
            });
        }

        private static async Task CreateLevel3(BotDatabaseContext context)
        {
            await context.Levels.AddAsync(new Level()
            {
                Id = 4, LevelValue = 3, LevelExpGoal = 127, LevelReward = new LevelReward()
                {
                    CashCapacity = 50, RandomPacks = 4, RandomStickerTier = 2
                }
            });
        }

        private static async Task CreateLevel2(BotDatabaseContext context)
        {
            await context.Levels.AddAsync(new Level()
            {
                Id = 3, LevelValue = 2, LevelExpGoal = 57, LevelReward = new LevelReward()
                {
                    CashCapacity = 50, RandomPacks = 4
                }
            });
        }

        private static async Task CreateLevel1(BotDatabaseContext context)
        {
            await context.Levels.AddAsync(new Level()
            {
                Id = 2, LevelValue = 1, LevelExpGoal = 25, LevelReward = new LevelReward()
                {
                    CashCapacity = 50, RandomPacks = 5, RandomStickerTier = 1
                }
            });
        }

        private static async Task CreateLevel0(BotDatabaseContext context)
        {
            await context.Levels.AddAsync(new Level()
            {
                Id = 1, LevelValue = 0, LevelExpGoal = 0, LevelReward = new LevelReward()
            });
        }
    }
}