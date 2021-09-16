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
        /* Таблица stickers в представлении Entity Framework */
        private static readonly DbSet<StickerEntity> Table = CardCollectorDatabase.Instance.Stickers;
        
        /* Получение информации о стикере по его Id, возвращает Null, если стикера не существует */
        public static async Task<StickerEntity> GetStickerInfo(string stickerId)
        {
            return await Table.FindAsync(stickerId);
        }

         /* Добавление новго стикера в систему
         fileId - id стикера на сервере telegram
         title - название стикера
         author - автор
         incomeCoins - прибыль стикера в монетах / минуту
         incomeGems - прибыль стикера в алзмазах / минуту
         tier - количество звезд стикера
         emoji - эмоции, связанные со стикером
         description - описание стикера */
        private static async Task<StickerEntity> AddNew(string fileId, string title, string author,
            int incomeCoins = 0, int incomeGems = 0, int tier = 1, string emoji = "", string description = "")

        {
            var cash = new StickerEntity
            {
                Id = fileId, Title = title, Author = author,
                IncomeCoins = incomeCoins, IncomeGems = incomeGems,
                Tier = tier, Emoji = emoji, Description = description
            };
            var result = await Table.AddAsync(cash);
            return result.Entity;
        }

        public static async Task<List<string>> GetAuthorsList()
        {
            return await Table
                .Select(item => item.Author)
                .Distinct()
                .ToListAsync();
        }
    }
}