using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class ShopDao
    {
        private static readonly CardCollectorDatabase Instance = CardCollectorDatabase.GetSpecificInstance(typeof(ShopDao));
        private static readonly DbSet<ShopEntity> Table = Instance.Shop;

        public static async Task<IEnumerable<ShopEntity>> GetShopPositions()
        {
            return await Table.WhereAsync(e => !e.IsSpecial);
        }
        
        public static async Task<IEnumerable<ShopEntity>> GetSpecialPositions()
        {
            return await Table.WhereAsync(e => e.IsSpecial && !e.Expired);
        }

        public static async Task<ShopEntity> GetById(int positionId)
        {
            return await Table.FirstAsync(e => e.Id == positionId);
        }

        public static async void DeleteRow(int productId)
        {
            if (await Table.FirstOrDefaultAsync(c => c.Id == productId) is not { } item) return;
            Table.Attach(item);
            Table.Remove(item);
            await Instance.SaveChangesAsync();
        }
    }
}