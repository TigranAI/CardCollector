using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class AuctionDao
    {
        /* Таблица stickers в представлении Entity Framework */
        private static readonly DbSet<StickerEntity> Table = CardCollectorDatabase.Instance.Stickers;
        
    }
}