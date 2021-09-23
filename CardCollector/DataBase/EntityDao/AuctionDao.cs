using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class AuctionDao
    {
        /* Таблица stickers в представлении Entity Framework */
        private static readonly DbSet<AuctionEntity> Table = CardCollectorDatabase.Instance.Auction;
        
        public static async Task<List<AuctionEntity>> GetProducts(string stickerId)
        {
            /* Заменил цикл на LINQ выражение и тип возвращаемого значение - список позиций,
             так как один и тот же стикер может продавать несколько людей */
            return await Table
                .Where(e => e.StickerId == stickerId).ToListAsync();
        }

        public static async Task<int> GetTotalQuantity(string stickerId)
        {
            /* Добавил метод, который считает общее количество данных стикеров на аукционе */
            var list = await GetProducts(stickerId);
            return list.Sum(e => e.Quantity);
        }

        public static async Task<IEnumerable<StickerEntity>> GetStickers(string filter)
        {
            var entityList = Table
                .Select(e => e.StickerId)
                .Distinct()
                .ToHashSet();
            var stickersList = await StickerDao.GetAll(filter);
            return stickersList.Where(e => entityList.Contains(e.Id));
        }
    }
}