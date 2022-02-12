using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.Entity.NotMapped;
using CardCollector.Resources;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.Migrations
{
    public static class DatabaseMigration
    {
        public static async Task Migrate(BotDatabaseContext context)
        {
            await InitPacks(context);
            await InitLevels(context);
            
        }
        
        private static async Task InitPacks(BotDatabaseContext context)
        {
            if (!await context.Packs.AnyAsync(pack => pack.Id == 1))
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
            if (!await context.Levels.AnyAsync(level => level.Id == 11))
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
            if (!await context.Levels.AnyAsync(level => level.Id == 10))
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
            if (!await context.Levels.AnyAsync(level => level.Id == 9))
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
            if (!await context.Levels.AnyAsync(level => level.Id == 8))
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
            if (!await context.Levels.AnyAsync(level => level.Id == 7))
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
            if (!await context.Levels.AnyAsync(level => level.Id == 6))
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
            if (!await context.Levels.AnyAsync(level => level.Id == 5))
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
            if (!await context.Levels.AnyAsync(level => level.Id == 4))
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
            if (!await context.Levels.AnyAsync(level => level.Id == 3))
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
            if (!await context.Levels.AnyAsync(level => level.Id == 2))
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
            if (!await context.Levels.AnyAsync(level => level.Id == 1))
                await context.Levels.AddAsync(new Level()
                {
                    Id = 1, LevelValue = 0, LevelExpGoal = 0, LevelReward = new LevelReward()
                });
        }
    }
}