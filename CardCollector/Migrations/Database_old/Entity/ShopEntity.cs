using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.Migrations.Database_old.Entity
{
    [Table("shop")]
    public class ShopEntity
    {
        [Key] [Column("id"), MaxLength(32)] public int Id { get; set; }
        [Column("pack_id"), MaxLength(32)] public int PackId { get; set; }
        [Column("title"), MaxLength(256)] public string Title { get; set; }
        [Column("is_infinite"), MaxLength(11)] public bool IsInfinite { get; set; }
        [Column("count"), MaxLength(32)] public int Count { get; set; }
        [Column("price_coins"), MaxLength(32)] public int PriceCoins { get; set; }
        [Column("price_gems"), MaxLength(32)] public int PriceGems { get; set; }
        [Column("discount"), MaxLength(32)] public int Discount { get; set; }
        [Column("time_limited"), MaxLength(32)] public bool TimeLimited { get; set; }
        [Column("time_limit"), MaxLength(32)] public DateTime TimeLimit { get; set; }
        [Column("additional_prize"), MaxLength(256)] public string? AdditionalPrize { get; set; }
        [Column("image_id"), MaxLength(127)] public string? ImageId { get; set; }
        [Column("description"), MaxLength(256)] public string? Description { get; set; }
    }
}