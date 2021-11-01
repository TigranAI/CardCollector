#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class PacksDao
    {
        public static async Task<PackEntity> GetById(int id)
        {
            var Table = BotDatabase.Instance.Packs;
            return await Table.FirstOrDefaultAsync(item => item.Id == id);
        }
        
        public static async Task<PackEntity> AddNew(string author, string description = "")
        {
            var Table = BotDatabase.Instance.Packs;
            var result = await Table.AddAsync(new PackEntity
            {
                Author = author,
                Description = description
            });
            await BotDatabase.SaveData();
            return result.Entity;
        }

        public static async Task<List<PackEntity>> GetAll()
        {
            var Table = BotDatabase.Instance.Packs;
            var list = await Table.Where(item => item.Id != 1).ToListAsync();
            list.Sort(new AuthorComparer());
            return list;
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