using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DailyTasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.Entity.NotMapped;
using CardCollector.Resources;
using CardCollector.StickerEffects;
using CardCollector.UserDailyTask;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using DailyTask = CardCollector.DataBase.Entity.DailyTask;

namespace CardCollector.DataBase
{
    using static AppSettings;

    public class BotDatabaseContext : DbContext
    {
        private double _id;

        public BotDatabaseContext()
        {
            _id = DateTime.Now.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            ).TotalMilliseconds;
            ContextManager.AddNewContext(_id, this);
        }

        public override ValueTask DisposeAsync()
        {
            ContextManager.DeleteContext(_id);
            return base.DisposeAsync();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Cash> Cash { get; set; }
        public DbSet<UserSticker> UserStickers { get; set; }
        public DbSet<Sticker> Stickers { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<SpecialOrder> SpecialOrders { get; set; }
        public DbSet<DailyTask> DailyTasks { get; set; }
        public DbSet<UserPacks> UsersPacks { get; set; }
        public DbSet<Pack> Packs { get; set; }
        public DbSet<SpecialOrderUser> SpecialOrderUsers { get; set; }
        public DbSet<UserLevel> UserLevels { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<UserSettings> Settings { get; set; }
        public DbSet<CountLogs> CountLogs { get; set; }
        public DbSet<UserMessages> UserMessages { get; set; }
        public DbSet<Payment> Payments { get; set; }

        /* Конфигурация подключения к БД */
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(
                $"server={DB_IP};" +
                $"port={DB_PORT};" +
                $"database={DB_SCHEMA};" +
                $"uid={DB_UID};" +
                $"pwd={DB_PWD}"
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureUserPrivilegeLevel(modelBuilder);
            ConfigureUserSettings(modelBuilder);
            ConfigureUserMessages(modelBuilder);
            ConfigureUserCash(modelBuilder);
            ConfigureUserLevel(modelBuilder);
            ConfigureLevelLevelReward(modelBuilder);
            ConfigureDailyTaskTaskId(modelBuilder);
            ConfigureStickerEffect(modelBuilder);
        }

        private void ConfigureUserLevel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .OwnsOne(user => user.Level);
        }

        private void ConfigureUserCash(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .OwnsOne(user => user.Cash);
        }

        private void ConfigureStickerEffect(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Sticker>()
                .Property(entity => entity.Effect)
                .HasConversion(
                    to => (int) to,
                    from => (Effect) from);
        }

        private void ConfigureUserMessages(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .OwnsOne(user => user.Messages, messages =>
                {
                    messages.Property(entity => entity.ChatMessages)
                        .HasConversion(
                            to => Utilities.ToJson(to),
                            from => Utilities.FromJson<HashSet<int>>(from),
                            new ValueComparer<ICollection<int>>(
                                (l1, l2) => l2 != null && l1 != null && l1.SequenceEqual(l2),
                                l => l.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                                l => l.ToHashSet()));
                    messages.Property(entity => entity.ChatStickers)
                        .HasConversion(
                            to => Utilities.ToJson(to),
                            from => Utilities.FromJson<List<int>>(from),
                            new ValueComparer<ICollection<int>>(
                                (l1, l2) => l2 != null && l1 != null && l1.SequenceEqual(l2),
                                l => l.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                                l => l.ToHashSet()));
                });
        }

        private void ConfigureDailyTaskTaskId(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<DailyTask>()
                .Property(entity => entity.TaskId)
                .HasConversion(
                    to => (int) to,
                    from => (TaskKeys) from);
        }

        private void ConfigureLevelLevelReward(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Level>()
                .Property(entity => entity.LevelReward)
                .HasConversion(
                    to => Utilities.ToJson(to),
                    from => Utilities.FromJson<LevelReward>(from));
        }

        private void ConfigureUserSettings(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .OwnsOne(user => user.Settings)
                .Property(entity => entity.Settings)
                .HasConversion(
                    to => Utilities.ToJson(to.ToDictionary(pair => (int) pair.Key, pair => pair.Value)),
                    from => Utilities.FromJson<Dictionary<int, bool>>(from)
                        .ToDictionary(pair => (UserSettingsEnum) pair.Key, pair => pair.Value),
                    new ValueComparer<Dictionary<UserSettingsEnum, bool>>(
                        (l1, l2) => l2 != null && l1 != null && l1.Values.SequenceEqual(l2.Values),
                        l => l.Aggregate(0, (a, v) =>
                            HashCode.Combine(a, HashCode.Combine(v.Key.GetHashCode(), v.Value.GetHashCode()))),
                        l => l.ToDictionary(p => p.Key, p => p.Value)));
        }

        private void ConfigureUserPrivilegeLevel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .Property(entity => entity.PrivilegeLevel)
                .HasConversion(
                    to => (int) to,
                    from => (PrivilegeLevel) from);
        }
    }
}