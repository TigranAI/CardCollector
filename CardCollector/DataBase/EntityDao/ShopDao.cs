using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class ShopDao
    {
        public static BotDatabase Instance;
        public static DbSet<ShopEntity> Table;

        static ShopDao()
        {
            Instance = BotDatabase.GetClassInstance(typeof(ShopDao));
            Table = Instance.Shop;
        }
        
        public static async Task<IEnumerable<ShopEntity>> GetShopPositions()
        {
            try
            {
                return (await Table.ToListAsync()).Where(e => !e.IsSpecial);
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetShopPositions();
            }
        }
        
        public static async Task<IEnumerable<ShopEntity>> GetSpecialPositions()
        {
            try
            {
                return (await Table.ToListAsync()).Where(e => e.IsSpecial && !e.Expired);
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetSpecialPositions();
            }
        }

        public static async Task<ShopEntity> GetById(int positionId)
        {
            try
            {
                return await Table.FirstAsync(e => e.Id == positionId);
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetById(positionId);
            }
        }
    }
}