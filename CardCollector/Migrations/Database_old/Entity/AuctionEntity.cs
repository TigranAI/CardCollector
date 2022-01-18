using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.Migrations.Database_old.Entity
{
    [Table("auction")]
    public class AuctionEntity
    {
        [Key] [Column("id"), MaxLength(32)] public int Id { get; set; }
        [Column("sticker_id"), MaxLength(127)] public string StickerId { get; set; }
        [Column("price"), MaxLength(32)] public int Price { get; set; }
        [Column("count"), MaxLength(32)] public int Count { get; set; }
        [Column("trader"), MaxLength(127)] public long Trader { get; set; }
    }
}