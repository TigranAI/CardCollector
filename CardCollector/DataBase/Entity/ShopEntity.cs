using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.DataBase.Entity
{
    [Table("shop")]
    public class ShopEntity
    {
        [Key] [Column("id"), MaxLength(32)] public int Id { get; set; }
        [Column("pack_id"), MaxLength(32)] public int PackId { get; set; } = 0;
        [Column("title"), MaxLength(256)] public string Title { get; set; } = "";
        [Column("is_infinite"), MaxLength(11)] public bool IsInfinite { get; set; } = true;
        [Column("count"), MaxLength(32)] public int Count { get; set; } = 1;
        [Column("price_coins"), MaxLength(32)] public int PriceCoins { get; set; } = 1000;
        [Column("price_gems"), MaxLength(32)] public int PriceGems { get; set; } = 80;
        [Column("discount"), MaxLength(32)] public int Discount { get; set; } = 0;
        [Column("time_limited"), MaxLength(32)] public bool TimeLimited { get; set; } = false;
        [Column("time_limit"), MaxLength(32)] public DateTime TimeLimit { get; set; } = DateTime.Now;
        [Column("additional_prize"), MaxLength(256)] public string AdditionalPrize { get; set; } = "";
        [Column("image_id"), MaxLength(127)] public string ImageId { get; set; } = "";
        [Column("description"), MaxLength(256)] public string Description { get; set; } = "";

        [NotMapped] public bool IsSpecial => Count > 1 || Discount > 0 || TimeLimited ||
                                             AdditionalPrize != "" || PackId is not 1 and not 2;
        [NotMapped] public bool Expired => TimeLimited && TimeLimit < DateTime.Now;
        [NotMapped] public int ResultPriceCoins => PriceCoins < 0 ? -1 : PriceCoins - PriceCoins * Discount / 100;
        [NotMapped] public int ResultPriceGems => PriceGems < 0 ? -1 : PriceGems - PriceGems * Discount / 100;
        
    }
}