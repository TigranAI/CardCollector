using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class AuctionDao
    {
        public static BotDatabase Instance;
        public static DbSet<AuctionEntity> Table;

        static AuctionDao()
        {
            Instance = BotDatabase.GetClassInstance(typeof(AuctionDao));
            Table = Instance.Auction;
        }
        
        public static async Task<List<AuctionEntity>> GetProducts(string stickerId)
        {
            try
            {
                return await Table.Where(e => e.StickerId == stickerId).ToListAsync();
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetProducts(stickerId);
            }
        }

        public static async Task<int> GetTotalQuantity(string stickerId)
        {
            try
            {
                var list = await GetProducts(stickerId);
                return list.Sum(e => e.Count);
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetTotalQuantity(stickerId);
            }
        }

        public static async Task<IEnumerable<StickerEntity>> GetStickers(string filter)
        {
            try
            {
                var entityList = (await Table.ToListAsync())
                    .Select(async e => await StickerDao.GetById(e.StickerId));
                var stickersList = await Task.WhenAll(entityList);
                return stickersList
                    .Where(item => item.Contains(filter))
                    .GroupBy(item => item.Id)
                    .Select(item => item.First());
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetStickers(filter);
            }
        }
        //добавляем объект в аукцион
        public static async void AddNew(AuctionEntity product)
        {
            try
            {
                await Table.AddAsync(product);
                await BotDatabase.SaveData();
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                AddNew(product);
            }
        }
        //удаляем проданный объект
        public static async Task DeleteRow(int productId)
        {
            try
            {
                if (await Table.FirstOrDefaultAsync(c => c.Id == productId) is not { } item) return;
                Table.Attach(item);
                Table.Remove(item);
                await BotDatabase.SaveData();
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                await DeleteRow(productId);
            }
        }

        public static async Task<bool> HaveAny(string stickerId, Expression<Func<AuctionEntity, bool>> source)
        {
            try
            {
                return await Table.Where(i => i.StickerId == stickerId).AnyAsync(source);
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await HaveAny(stickerId, source);
            }
        }

        public static async Task<int> GetCount(int productId)
        {
            try
            {
                return (await Table.FirstAsync(item => item.Id == productId)).Count;
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetCount(productId);
            }
        }

        public static async Task<AuctionEntity> GetProduct(int productId)
        {
            try
            {
                return await Table.FirstAsync(item => item.Id == productId);
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetProduct(productId);
            }
        }
    }
}