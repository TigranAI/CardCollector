using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class ShopDao
    {
        public static async Task<IEnumerable<ShopEntity>> GetShopPositions()
        {
            var Table = BotDatabase.Instance.Shop;
            return (await Table.ToListAsync()).Where(e => !e.IsSpecial);
        }
        
        public static async Task<IEnumerable<ShopEntity>> GetSpecialPositions()
        {
            var Table = BotDatabase.Instance.Shop;
            return (await Table.ToListAsync()).Where(e => e.IsSpecial && !e.Expired);
        }

        public static async Task<ShopEntity> GetById(int positionId)
        {
            var Table = BotDatabase.Instance.Shop;
            return await Table.FirstAsync(e => e.Id == positionId);
        }
    }
}