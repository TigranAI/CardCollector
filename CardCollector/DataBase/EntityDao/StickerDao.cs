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

        public static async Task<List<string>> GetAuthorsList()
        {
            try
            {
                var list = await Table.Select(item => item.Author).Distinct().ToListAsync();
                list.Sort();
                return list;
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetAuthorsList();
            }
        }

        public static async Task<List<StickerEntity>> GetAll(string filter = "")
        {
            try
            {
                return await Table.Where(item => item.Contains(filter)).ToListAsync();
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await GetAll(filter);
            }
        }

        public static async Task AddNew(StickerEntity sticker)
        {
            try
            {
                await Table.AddAsync(sticker);
                await BotDatabase.SaveData();
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                await AddNew(sticker);
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

        public static async Task<List<StickerEntity>> GetListWhere(Expression<Func<StickerEntity, bool>> func)
        {
            try
            {
                return await Table.Where(func).ToListAsync();
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
    }
}