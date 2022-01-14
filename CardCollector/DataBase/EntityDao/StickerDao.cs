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
    /* Класс, предоставляющий доступ к объектам таблицы Stickers*/
    public static class StickerDao
    {
        public static BotDatabase Instance;
        public static DbSet<StickerEntity> Table;

        static StickerDao()
        {
            Instance = BotDatabase.GetClassInstance(typeof(StickerDao));
            Table = Instance.Stickers;
        }
        
        /* Получение информации о стикере по его хешу, возвращает Null, если стикера не существует */
        public static async Task<StickerEntity> GetByHash(string hash)
        {
            try
            {
                return await Table.FirstOrDefaultAsync(item => item.Md5Hash == hash);
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetByHash(hash);
            }
        }

        public static async Task<List<StickerEntity>> GetAll(string filter = "")
        {
            try
            {
                return (await Table.ToListAsync()).Where(item => item.Contains(filter)).ToList();
            }
            catch (InvalidOperationException e)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetAll(filter);
            }
        }

        public static async Task<StickerEntity> AddNew(StickerEntity sticker)
        {
            try
            {
                var result = await Table.AddAsync(sticker);
                await BotDatabase.SaveData();
                return result.Entity;
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await AddNew(sticker);
            }
        }

        public static async Task AddRange(IEnumerable<StickerEntity> stickers, int packId)
        {
            try
            {
                await Table.AddRangeAsync(stickers.Select(item => {
                    item.PackId = packId;
                    return item;
                }));
                await BotDatabase.SaveData();
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                await AddRange(stickers, packId);
            }
        }

        public static async Task<List<StickerEntity>> GetListWhere(Func<StickerEntity, bool> func)
        {
            try
            {
                return (await Table.ToListAsync()).Where(func).ToList();
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetListWhere(func);
            }
        }

        public static async Task<StickerEntity> GetById(string id)
        {
            try
            {
                return await Table.FirstOrDefaultAsync(sticker => sticker.Id == id);
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetById(id);
            }
        }

        public static async Task DeleteSticker(StickerEntity sticker)
        {
            try
            {
                Table.Attach(sticker);
                Table.Remove(sticker);
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                await DeleteSticker(sticker);
            }
        }
    }
}