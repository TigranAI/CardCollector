using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace CardCollector.DataBase
{
    using static Resources.AppSettings;
    
    /* Предоставляет доступ к базе данных */
    public class BotDatabase : DbContext
    {
        /* Скрываем конструктор, чтобы его нельзя было использовать извне */
        private BotDatabase() { }
        protected DateTime _lastSave = DateTime.Now;
        
        /* Объект базы данных */
        private static List<BotDatabase> _instances = new();
        
        /* Предоставляет доступ к объекту */
        public static BotDatabase Instance
        {
            get
            {
                var instance = new BotDatabase();
                _instances.Add(instance);
                return instance;
            }
        }

        /* Таблицы базы данных, представленные Entity объектами */
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<CashEntity> Cash { get; set; }
        public DbSet<UserStickerRelationEntity> UserStickerRelations { get; set; }
        public DbSet<StickerEntity> Stickers { get; set; }
        public DbSet<AuctionEntity> Auction { get; set; }
        public DbSet<ShopEntity> Shop { get; set; }
        public DbSet<DailyTaskEntity> DailyTasks { get; set; }
        public DbSet<UserPacks> UsersPacks { get; set; }
        public DbSet<PackEntity> Packs { get; set; }
        public DbSet<SpecialOfferUsers> SpecialOfferUsers { get; set; }
        public DbSet<SessionToken> SessionTokens { get; set; }
        public DbSet<UserLevel> UserLevel { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<UserSettings> Settings { get; set; }

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

        public static async Task SaveData()
        {
            foreach (var instance in _instances.ToList())
            {
                await instance.SaveChangesAsync();
                var activityInterval = instance._lastSave - DateTime.Now;
                if (activityInterval.TotalMinutes > 120)
                {
                    _instances.Remove(instance);
                    await instance.DisposeAsync();
                }
            }
        }

        public override void Dispose()
        {
            SaveChanges();
            base.Dispose();
        }

        public override async ValueTask DisposeAsync()
        {
            await SaveChangesAsync();
            await base.DisposeAsync();
        }

        public override int SaveChanges()
        { 
            var count = base.SaveChanges();
            if (count > 0) _lastSave = DateTime.Now;
            return count;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var count = await base.SaveChangesAsync(cancellationToken);
            if (count > 0) _lastSave = DateTime.Now;
            return count;
        }
    }
}