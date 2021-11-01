using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class AuctionDao
    {
        public static async Task<List<AuctionEntity>> GetProducts(string stickerId)
        {
            var Table = BotDatabase.Instance.Auction;
            return await Table.Where(e => e.StickerId == stickerId).ToListAsync();
        }

        public static async Task<int> GetTotalQuantity(string stickerId)
        {
            /* Добавил метод, который считает общее количество данных стикеров на аукционе */
            var list = await GetProducts(stickerId);
            return list.Sum(e => e.Count);
        }

        public static async Task<IEnumerable<StickerEntity>> GetStickers(string filter)
        {
            var Table = BotDatabase.Instance.Auction;
            var entityList = (await Table.ToListAsync())
                .Select(async e => await StickerDao.GetById(e.StickerId));
            var stickersList = await Task.WhenAll(entityList);
            return stickersList
                .Where(item => item.Contains(filter))
                .GroupBy(item => item.Id)
                .Select(item => item.First());
        }
        //добавляем объект в аукцион
        public static async void AddNew(AuctionEntity product)
        {
            var Table = BotDatabase.Instance.Auction;
            await Table.AddAsync(product);
            await BotDatabase.SaveData();
        }
        //удаляем проданный объект
        public static async Task DeleteRow(int productId)
        {
            var Table = BotDatabase.Instance.Auction;
            if (await Table.FirstOrDefaultAsync(c => c.Id == productId) is not { } item) return;
            Table.Attach(item);
            Table.Remove(item);
            await BotDatabase.SaveData();
        }

        public static async Task<bool> HaveAny(string stickerId, Expression<Func<AuctionEntity, bool>> source)
        {
            var Table = BotDatabase.Instance.Auction;
            return await Table.Where(i => i.StickerId == stickerId).AnyAsync(source);
        }

        public static async Task<int> GetCount(int productId)
        {
            var Table = BotDatabase.Instance.Auction;
            return (await Table.FirstAsync(item => item.Id == productId)).Count;
        }

        public static async Task<AuctionEntity> GetProduct(int productId)
        {
            var Table = BotDatabase.Instance.Auction;
            return await Table.FirstAsync(item => item.Id == productId);
        }
    }
}