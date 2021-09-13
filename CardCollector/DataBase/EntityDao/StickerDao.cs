﻿using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class StickerDao
    {
        private static readonly DbSet<StickerEntity> Table = CardCollectorDatabase.Instance.Stickers;
        
        public static async Task<StickerEntity> GetStickerInfo(string stickerId)
        {
            return await Table.FindAsync(stickerId);
        }

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
    }
}