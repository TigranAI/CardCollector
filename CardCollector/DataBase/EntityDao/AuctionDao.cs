using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.Others;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class AuctionDao
    {
        private static readonly CardCollectorDatabase Instance = CardCollectorDatabase.GetSpecificInstance(typeof(AuctionDao));
        /* Таблица auction в представлении Entity Framework */
        private static readonly DbSet<AuctionEntity> Table = Instance.Auction;
        
        public static async Task<List<AuctionEntity>> GetProducts(string stickerId)
        {
            /* Заменил цикл на LINQ выражение и тип возвращаемого значения - список позиций,
             так как один и тот же стикер может продавать несколько людей */
            return (await Table.WhereAsync(e => Task.FromResult(e.StickerId == stickerId))).ToList();
        }

        public static async Task<TraderInformation> GetTraderInfo(int productId)
        {
            var product = await Table.FirstAsync(item => item.Id == productId);
            var trader = await UserDao.GetById(product.Trader);
            return new TraderInformation(product) {Username = trader.Username};
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
        //добавляем объект в аукцион
        public static async void AddNew(AuctionEntity product)
        {
            await Table.AddAsync(product);
            await Instance.SaveChangesAsync();
        }
        //удаляем проданный объект
        public static async Task DeleteRow(int productId)
        {
            if (await Table.FirstOrDefaultAsync(c => c.Id == productId) is not { } item) return;
            Table.Attach(item);
            Table.Remove(item);
            await Instance.SaveChangesAsync();
        }

        public static bool HaveAny(string stickerId, Expression<Func<AuctionEntity, bool>> source)
        {
            return Table.Where(i => i.StickerId == stickerId).AnyAsync(source).Result;
        }

        public static async Task<int> GetQuantity(int productId)
        {
            return (await Table.FirstAsync(item => item.Id == productId)).Quantity;
        }

        public static async Task<AuctionEntity> GetProduct(int productId)
        {
            return await Table.FirstAsync(item => item.Id == productId);
        }
    }
}