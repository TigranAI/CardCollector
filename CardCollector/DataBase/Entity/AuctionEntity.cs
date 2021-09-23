using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.DataBase.Entity
{
    [Table("auction")]
    public class AuctionEntity
    {
        /* добавил, так как один и тот же стикер может продаваться разными людьми,
         следовательно - он не уникальный */
        /* id записи */
        [Column("id"), MaxLength(32)] public int Id { get; set; }
        
        /* id стикера */
        [Column("sticker_id"), MaxLength(127)] public string StickerId { get; set; }
        
        /* Разбил на 2 отдельных цены, так как я ранее говорил,
         что можно будет продать стик за 2 валюты одновременно, поле валюты упразднил */
        /* цена в монетах */
        [Column("price_coins"), MaxLength(32)] public int PriceCoins { get; set; }
        
        /* цена в алмазах */
        [Column("price_gems"), MaxLength(32)] public int PriceGems { get; set; }
        
        /* количество */
        [Column("quantity"), MaxLength(32)] public int Quantity { get; set; }
        
        /* продавец */
        [Column("trader"), MaxLength(127)] public long Trader { get; set; }
    }
}