using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase
{
    using static Resources.AppSettings;
    public class CardCollectorDatabase : DbContext
    {
        private CardCollectorDatabase() { }
        private static CardCollectorDatabase _instance;
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
        
        // Таблицы базы данных, представленные Entity объектами
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<CashEntity> CashTable { get; set; }
        public DbSet<UserStickerRelationEntity> UserStickerRelations { get; set; }
        public DbSet<StickerEntity> Stickers { get; set; }

        
        // Конфигурация подключения к БД
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