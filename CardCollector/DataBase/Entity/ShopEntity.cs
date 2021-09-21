using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.DataBase.Entity
{
    [Table("shop")]
    public class ShopEntity
    {
        [Key]
        [Column("id"), MaxLength(32)] public int Id { get; set; }
        [Column("sticker_id"), MaxLength(127)] public string StickerId { get; set; }
        [Column("count"), MaxLength(32)] public int Count { get; set; }
        [Column("is_infinite"), MaxLength(11)] public bool IsInfinite { get; set; }
    }
}