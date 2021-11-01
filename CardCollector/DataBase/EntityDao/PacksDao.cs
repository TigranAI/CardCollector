#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class PacksDao
    {
        public static BotDatabase Instance;
        public static DbSet<PackEntity> Table;

        static PacksDao()
        {
            Instance = BotDatabase.GetClassInstance(typeof(PacksDao));
            Table = Instance.Packs;
        }
    
        public static async Task<PackEntity> GetById(int id)
        {
            try
            {
                return await Table.FirstOrDefaultAsync(item => item.Id == id);
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetById(id);
            }
        }
        
        public static async Task<PackEntity> AddNew(string author, string description = "")
        {
            try
            {
                var result = await Table.AddAsync(new PackEntity
                {
                    Author = author,
                    Description = description
                });
                await BotDatabase.SaveData();
                return result.Entity;
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await AddNew(author, description);
            }
            
        }

        public static async Task<List<PackEntity>> GetAll()
        {
            try
            {
                var list = await Table.Where(item => item.Id != 1).ToListAsync();
                list.Sort(new AuthorComparer());
                return list;
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetAll();
            }
            
        }

        private class AuthorComparer : IComparer<PackEntity>
        {
            public int Compare(PackEntity? x, PackEntity? y)
            {
                if (x is null || y is null) return 0;
                return string.Compare(x.Author, y.Author, StringComparison.CurrentCultureIgnoreCase);
            }
        }
    }
}