using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;

namespace CardCollector.DataBase.Entity
{
    [Table("auction")]
    public class AuctionEntity
    {
        /* id записи */
        [Key] [Column("id"), MaxLength(32)] public int Id { get; set; }
        
        /* id стикера */
        [Column("sticker_id"), MaxLength(127)] public string StickerId { get; set; }

        /* цена (в алмазах) */
        [Column("price"), MaxLength(32)] public int Price { get; set; }
        
        /* количество */
        [Column("count"), MaxLength(32)] public int Count { get; set; }
        
        /* продавец */
        [Column("trader"), MaxLength(127)] public long Trader { get; set; }

        public async Task BuyCard(int count)
        {
            Count -= count;
            var user = await UserDao.GetById(Trader);
            var sticker = await StickerDao.GetById(StickerId);
            var gemsSum = (int)(Price * count * 0.7);
            await MessageController.SendMessage(user, $"{Messages.you_sold} {sticker.Title} {count}{Text.items}" +
                                                      $"\n{Messages.you_collected} {gemsSum}{Text.gem}");
            user.Cash.Gems += gemsSum;
            await CashDao.Save();
            if (Count == 0) await AuctionDao.DeleteRow(Id);
        }
    }
}