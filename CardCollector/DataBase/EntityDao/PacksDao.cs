using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class PacksDao
    {
        private static readonly CardCollectorDatabase Instance = CardCollectorDatabase.GetSpecificInstance(typeof(PacksDao));
        private static readonly DbSet<PackEntity> Table = Instance.Packs;

        public static async Task<PackEntity> GetById(int id)
        {
            return await Table.FirstOrDefaultAsync(item => item.Id == id);
        }
        
        public static async Task<PackEntity> AddNew(string author, string description = "")
        {
            var result = await Table.AddAsync(new PackEntity
            {
                Author = author,
                Description = description
            });
            await Instance.SaveChangesAsync();
            return result.Entity;
        }

        public static async Task<List<PackEntity>> GetAll()
        {
            return (await Table.WhereAsync(item => item.Id is not 1 or 2)).ToList();
        }
    }
}