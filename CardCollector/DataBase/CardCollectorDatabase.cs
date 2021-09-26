using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase
{
    using static Resources.AppSettings;
    
    /* Предоставляет доступ к базе данных */
    public class CardCollectorDatabase : DbContext
    {
        /* Скрываем конструктор, чтобы его нельзя было использовать извне */
        private CardCollectorDatabase() { }
        
        /* Объект базы данных */
        private static CardCollectorDatabase _instance;
        
        /* Предоставляет доступ к объекту */
        public static CardCollectorDatabase Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new CardCollectorDatabase();
                _instance.Database.EnsureCreated();
                return _instance;
            }
        }

        private static readonly Dictionary<Type, CardCollectorDatabase> _specificInstances = new ();

        public static CardCollectorDatabase GetSpecificInstance(Type type)
        {
            try
            {
                return _specificInstances[type];
            }
            catch (Exception)
            {
                var newInstance = new CardCollectorDatabase();
                _specificInstances.Add(type, newInstance);
                newInstance.Database.EnsureCreated();
                return newInstance;
            }
        }

        public static async Task SaveAllChangesAsync()
        {
            try
            {
                await Instance.SaveChangesAsync();
                foreach (var instance in _specificInstances.Values)
                    await instance.SaveChangesAsync();
            } catch (Exception) { /* Ignored */ }
        }

        /* Таблицы базы данных, представленные Entity объектами */
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<CashEntity> CashTable { get; set; }
        public DbSet<UserStickerRelationEntity> UserStickerRelations { get; set; }
        public DbSet<StickerEntity> Stickers { get; set; }
        public DbSet<AuctionEntity> Auction { get; set; }
        public DbSet<ShopEntity> Shop { get; set; }

        
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
    }
}