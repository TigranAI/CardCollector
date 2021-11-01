using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    /* Класс, предоставляющий доступ к объектам таблицы Stickers*/
    public static class StickerDao
    {
        /* Получение информации о стикере по его хешу, возвращает Null, если стикера не существует */
        public static async Task<StickerEntity> GetByHash(string hash)
        {
            var Table = BotDatabase.Instance.Stickers;
            return await Table.FirstOrDefaultAsync(item => item.Md5Hash == hash);
        }

        public static async Task<List<string>> GetAuthorsList()
        {
            var Table = BotDatabase.Instance.Stickers;
            var list = await Table.Select(item => item.Author).Distinct().ToListAsync();
            list.Sort();
            return list;
        }

        public static async Task<List<StickerEntity>> GetAll(string filter = "")
        {
            var Table = BotDatabase.Instance.Stickers;
            return await Table.Where(item => item.Contains(filter)).ToListAsync();
        }

        public static async Task AddNew(StickerEntity sticker)
        {
            var Table = BotDatabase.Instance.Stickers;
            await Table.AddAsync(sticker);
            await BotDatabase.SaveData();
        }

        public static async Task AddRange(IEnumerable<StickerEntity> stickers, int packId)
        {
            var Table = BotDatabase.Instance.Stickers;
            await Table.AddRangeAsync(stickers.Select(item => {
                item.PackId = packId;
                return item;
            }));
            await BotDatabase.SaveData();
        }

        public static async Task<List<StickerEntity>> GetListWhere(Expression<Func<StickerEntity, bool>> func)
        {
            var Table = BotDatabase.Instance.Stickers;
            return await Table.Where(func).ToListAsync();
        }

        public static async Task<StickerEntity> GetById(string id)
        {
            var Table = BotDatabase.Instance.Stickers;
            return await Table.FirstOrDefaultAsync(sticker => sticker.Id == id);
        }
    }
}