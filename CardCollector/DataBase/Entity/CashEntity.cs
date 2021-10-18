using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using CardCollector.DataBase.EntityDao;

namespace CardCollector.DataBase.Entity
{
    /* Объект таблицы cash (одна строка)
     В таблице хранится Id пользователя, количество монет и алмазов*/
    [Table("cash")]
    public class CashEntity
    {
        /* Id пользователя */
        [Key]
        [Column("user_id"), MaxLength(127)] public long UserId { get; set; }
        
        /* Количество монет */
        [Column("coins"), MaxLength(32)] public int Coins { get; set; } = 0;
        
        /* Количество алмазов */
        [Column("gems"), MaxLength(32)] public int Gems { get; set; } = 0;
        
        /* Размер копилки */
        [Column("max_capacity"), MaxLength(32)] public int MaxCapacity { get; set; } = 200;
        
        [NotMapped] private DateTime LastPayout = DateTime.Now;
        
        public async Task<int> CalculateIncome(Dictionary<string, UserStickerRelationEntity> stickers)
        {
            LastPayout = DateTime.Now;
            var result = await stickers.Values.SumAsync(async sticker => await Payout(sticker));
            return result > MaxCapacity ? MaxCapacity : result;
        }
        
        public async Task<int> Payout(Dictionary<string, UserStickerRelationEntity> stickers)
        {
            var result = await stickers.Values.SumAsync(async sticker => await Payout(sticker, true));
            result = result > MaxCapacity ? MaxCapacity : result;
            Coins += result;
            return result;
        }
        
        private async Task<int> Payout(UserStickerRelationEntity relation, bool updatePayout = false)
        {
            var stickerInfo = await StickerDao.GetById(relation.StickerId);
            var payoutInterval = LastPayout - relation.Payout;
            var payoutsCount = (int) (payoutInterval.TotalMinutes / stickerInfo.IncomeTime);
            if (updatePayout) relation.Payout = LastPayout;
            if (payoutsCount < 1) return 0;
            return stickerInfo.Income * payoutsCount * relation.Count;
        }
    }
}