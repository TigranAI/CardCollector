using CardCollector.Migrations.Database_old.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.Migrations.Database_old
{
    public class BotDatabase : DbContext
    {
        private string DB_IP = "localhost";
        private string DB_PORT = "3306";
        private string DB_SCHEMA = "card_collector";
        private string DB_UID = "card_collector";
        private string DB_PWD = "SSGiU4lcFz2V1*";
        
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<CashEntity> Cash { get; set; }
        public DbSet<UserStickerRelation> UserStickerRelations { get; set; }
        public DbSet<StickerEntity> Stickers { get; set; }
        public DbSet<AuctionEntity> Auction { get; set; }
        public DbSet<ShopEntity> Shop { get; set; }
        public DbSet<DailyTaskEntity> DailyTasks { get; set; }
        public DbSet<UserPacks> UsersPacks { get; set; }
        public DbSet<PackEntity> Packs { get; set; }
        public DbSet<SpecialOfferUsers> SpecialOfferUsers { get; set; }
        public DbSet<UserLevel> UserLevel { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<UserSettings> Settings { get; set; }
        public DbSet<CountLogs> CountLogs { get; set; }
        public DbSet<UserMessages> UserMessages { get; set; }
        public DbSet<Payment> Payments { get; set; }
        
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