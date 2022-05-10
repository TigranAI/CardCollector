using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Database.Entity;
using CardCollector.Database.Entity.NotMapped;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.UserDailyTask;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using UserSettings = CardCollector.Database.Entity.UserSettings;

namespace CardCollector.Database
{
    using static AppSettings;

    public class BotDatabaseContext : DbContext
    {
        private bool _disposed;
        private double _id;

        public BotDatabaseContext()
        {
            _id = DateTime.Now.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            ).TotalMilliseconds;
            ContextManager.AddNewContext(_id, this);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Cash> Cash { get; set; }
        public DbSet<UserSticker> UserStickers { get; set; }
        public DbSet<Sticker> Stickers { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<SpecialOrder> SpecialOrders { get; set; }
        public DbSet<DailyTask> DailyTasks { get; set; }
        public DbSet<UserPacks> UserPacks { get; set; }
        public DbSet<Pack> Packs { get; set; }
        public DbSet<SpecialOrderUser> SpecialOrderUsers { get; set; }
        public DbSet<UserLevel> UserLevels { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<UserSettings> Settings { get; set; }
        public DbSet<CountLogs> CountLogs { get; set; }
        public DbSet<UserMessages> UserMessages { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<UserSendStickerToChat> UserSendStickers { get; set; }
        public DbSet<TelegramChat> TelegramChats { get; set; }
        public DbSet<ChannelGiveaway> ChannelGiveaways { get; set; }
        public DbSet<ChatRoulette> ChatRoulette { get; set; }
        public DbSet<ChatActivity> ChatActivities { get; set; }
        public DbSet<InviteInfo> InviteInfo { get; set; }
        public DbSet<ChatDistribution> ChatDistributions { get; set; }

        public bool IsDisposed()
        {
            return _disposed;
        }

        /* Конфигурация подключения к БД */
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = $"server={DB_IP};port={DB_PORT};database={DB_SCHEMA};uid={DB_UID};pwd={DB_PWD}";
            optionsBuilder
                .UseMySql(connectionString,ServerVersion.AutoDetect(connectionString))
                .UseLazyLoadingProxies()
                .UseSnakeCaseNamingConvention();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureUserPrivilegeLevel(modelBuilder);
            ConfigureUserSettings(modelBuilder);
            ConfigureUserMessages(modelBuilder);
            ConfigureUserCash(modelBuilder);
            ConfigureUserLevel(modelBuilder);
            ConfigureUserInviteInfo(modelBuilder);
            ConfigureLevelLevelReward(modelBuilder);
            ConfigureDailyTaskTaskId(modelBuilder);
            ConfigureSticker(modelBuilder);
            ConfigureTelegramChat(modelBuilder);
            ConfigureChatDistributions(modelBuilder);
        }

        private void ConfigureChatDistributions(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<ChatDistribution>()
                .Property(cd => cd.Buttons)
                .HasConversion(new JsonValueConverter<List<ButtonInfo>>(),
                    new CollectionComparer<ButtonInfo>());
        }

        private void ConfigureUserInviteInfo(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .HasOne(u => u.InviteInfo)
                .WithOne(ii => ii.User);

            modelBuilder
                .Entity<InviteInfo>()
                .OwnsOne(ii => ii.TasksProgress,
                    builder => builder.ToTable("beginners_tasks_progress"));
        }

        private void ConfigureTelegramChat(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<TelegramChat>()
                .OwnsOne(chat => chat.ChatActivity, builder =>
                {
                    builder.ToTable("chat_activity");
                });
        }

        private void ConfigureUserLevel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .OwnsOne(user => user.Level, builder => builder.ToTable("user_level"));
        }

        private void ConfigureUserCash(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .OwnsOne(user => user.Cash, builder => builder.ToTable("user_cash"));
        }

        private void ConfigureSticker(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Sticker>()
                .Property(entity => entity.Effect)
                .HasConversion(
                    to => (int) to,
                    from => (Effect) from);
            
            modelBuilder
                .Entity<Sticker>()
                .Property(entity => entity.ExclusiveTask)
                .HasConversion(
                    to => (int) to,
                    from => (ExclusiveTask) from);
        }

        private void ConfigureUserMessages(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .OwnsOne(user => user.Messages, builder =>
                {
                    builder.ToTable("user_messages");
                    builder
                        .Property(entity => entity.ChatMessages)
                        .HasConversion(new JsonValueConverter<HashSet<int>>(),
                            new HashSetComparer<int>());
                    builder
                        .Property(entity => entity.ChatStickers)
                        .HasConversion(new JsonValueConverter<HashSet<int>>(),
                            new HashSetComparer<int>());
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
                .HasConversion(new JsonValueConverter<LevelReward>());
        }

        private void ConfigureUserSettings(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .OwnsOne(user => user.Settings, builder =>
                {
                    builder
                        .ToTable("user_settings")
                        .Property(entity => entity.Settings)
                        .HasConversion(
                            to => 
                                Utilities.ToJson(to.ToDictionary(pair => (int) pair.Key, pair => pair.Value)),
                            from => 
                                Utilities.FromJson<Dictionary<int, bool>>(from)
                                .ToDictionary(pair => (UserSettingsTypes) pair.Key, pair => pair.Value),
                            new ValueComparer<Dictionary<UserSettingsTypes, bool>>(
                                (l1, l2) => l2 != null && l1 != null && l1.Values.SequenceEqual(l2.Values),
                                l => l.Aggregate(0, (a, v) =>
                                    HashCode.Combine(a, HashCode.Combine(v.Key.GetHashCode(), v.Value.GetHashCode()))),
                                l => l.ToDictionary(p => p.Key, p => p.Value)));
                });
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

        public override ValueTask DisposeAsync()
        {
            _disposed = true;
            ContextManager.DeleteContext(_id);
            return base.DisposeAsync();
        }

        public override void Dispose()
        {
            _disposed = true;
            ContextManager.DeleteContext(_id);
            base.Dispose();
        }
    }
}