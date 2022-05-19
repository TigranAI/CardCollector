using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Database.Entity;
using CardCollector.Session.Modules;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.Database.EntityDao
{
    public static class AuctionDao
    {
        public static async Task<int> GetCountByStickerId(this DbSet<Auction> table, long stickerId,
            FiltersModule filters)
        {
            var list = await table.Where(item => item.Sticker.Id == stickerId).ToListAsync();
            if (filters.Author is { } author)
                list.RemoveAll(item => !item.Sticker.Author.Contains(author));
            if (filters.Tier is { } tier)
                list.RemoveAll(item => item.Sticker.Tier != tier);
            if (filters.Emoji is { } emoji)
                list.RemoveAll(item => !item.Sticker.Emoji.Contains(emoji));
            filters.ApplyPriceTo(list);
            return list.Sum(item => item.Count);
        }

        public static async Task<Auction?> FindById(this DbSet<Auction> table, long? id)
        {
            return await table.SingleOrDefaultAsync(item => item.Id == id);
        }

        public static async Task<List<Sticker>> FindAll(this DbSet<Auction> table, FiltersModule filters)
        {
            var list = (await table
                .Include(item => item.Sticker)
                .Where(item => item.Count > 0)
                .ToListAsync())
                .DistinctBy(item => item.Sticker.Id)
                .ToList();
            filters.ApplyPriceTo(list);
            var stickers = list.Select(item => item.Sticker).ToList();
            filters.ApplyTo(stickers);
            return stickers;
        }

        public static async Task<List<Auction>> FindAllOrders(this DbSet<Auction> table, long? stickerId,
            string query = "")
        {
            var list = await table
                .Where(item => item.Count > 0 && item.Sticker.Id == stickerId && item.Trader.Username.Contains(query))
                .ToListAsync();
            return list;
        }

        public static async Task<int> FindMinPriceByStickerId(this DbSet<Auction> table, long? stickerId)
        {
            try
            {
                return await table
                    .Where(item => item.Sticker.Id == stickerId)
                    .MinAsync(item => item.Price);
            }
            catch
            {
                return 0;
            }
        }

        public static async Task AddAsync(this DbSet<Auction> table, User trader, Sticker sticker, int count, int price)
        {
            var entity = new Auction()
            {
                Trader = trader,
                Sticker = sticker,
                Count = count,
                Price = price
            };
            await table.AddAsync(entity);
        }
    }
}