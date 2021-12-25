using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class UserPacksDao
    {
        public static BotDatabase Instance;
        public static DbSet<UserPacks> Table;

        static UserPacksDao()
        {
            Instance = BotDatabase.GetClassInstance(typeof(UserPacksDao));
            Table = Instance.UsersPacks;
        }
        
        public static async Task<List<UserPacks>> GetUserPacks(long userId)
        {
            try
            {
                return await Table.Where(item => item.UserId == userId).ToListAsync();
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetUserPacks(userId);
            }
        }

        public static async Task<UserPacks> AddNew(long userId, int packId)
        {
            try
            {
                var newPack = new UserPacks() { UserId = userId, PackId = packId };
                var result = await Table.AddAsync(newPack);
                await Instance.SaveChangesAsync();
                return result.Entity;
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await AddNew(userId, packId);
            }
        }

        public static async Task<UserPacks> GetOne(long userId, int packId)
        {
            try
            {
                return await Table.FirstOrDefaultAsync(item => item.UserId == userId && item.PackId == packId)
                       ?? await AddNew(userId, packId);
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetOne(userId, packId);
            }
        }

        public static async Task<int> GetCount(long userId)
        {
            try
            {
                var packs = await Table.Where(item => item.UserId == userId).ToListAsync();
                return packs.Sum(item => item.Count);
                
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetCount(userId);
            }
        }
    }
}