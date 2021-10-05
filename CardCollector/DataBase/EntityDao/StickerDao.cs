using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    /* Класс, предоставляющий доступ к объектам таблицы Stickers*/
    public static class StickerDao
    {
        private static readonly CardCollectorDatabase Instance = CardCollectorDatabase.GetSpecificInstance(typeof(StickerDao));
        /* Таблица stickers в представлении Entity Framework */
        private static readonly DbSet<StickerEntity> Table = Instance.Stickers;
        
        /* Получение информации о стикере по его хешу, возвращает Null, если стикера не существует */
        public static async Task<StickerEntity> GetStickerByHash(string hash)
        {
            return await Table.FirstOrDefaultAsync(item => item.Md5Hash == hash);
        }

        public static async Task<List<string>> GetAuthorsList()
        {
            return await Table
                .Select(item => item.Author)
                .Distinct()
                .ToListAsync();
        }

        public static async Task<List<StickerEntity>> GetAll(string filter = "")
        {
            var list = await Table.ToListAsync();
            return filter == "" ? list : list.Where
                (item => item.Title.Contains(filter, StringComparison.CurrentCultureIgnoreCase)).ToList();
        }

        public static async Task AddNew(StickerEntity sticker)
        {
            await Table.AddAsync(sticker);
        }

        public static async Task<List<StickerEntity>> GetListWhere(Func<StickerEntity, bool> func)
        {
            return (await Table.ToListAsync()).Where(func).ToList();
        }

        public static async Task<StickerEntity> GetById(string id)
        {
            return await Table.FirstOrDefaultAsync(sticker => sticker.Id == id);
        }
    }
}