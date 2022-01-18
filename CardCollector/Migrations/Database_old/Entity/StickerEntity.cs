using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.Migrations.Database_old.Entity
{
    [Table("stickers")]
    public class StickerEntity
    {
        [Column("id"), MaxLength(127)] public string Id { get; set; }
        [Column("for_sale_id"), MaxLength(127)] public string? ForSaleId { get; set; }
        [Column("title"), MaxLength(256)] public string Title { get; set; }
        [Column("author"), MaxLength(128)] public string Author { get; set; }
        [Column("income"), MaxLength(32)] public int Income { get; set; }
        [Column("income_time"), MaxLength(32)] public int IncomeTime { get; set; }
        [Column("tier"), MaxLength(32)] public int Tier { get; set; }
        [Column("emoji"), MaxLength(127)] public string Emoji { get; set; }
        [Column("description"), MaxLength(1024)] public string? Description { get; set; }
        [Column("md5hash"), MaxLength(40)] public string Md5Hash { get; set; }
        [Column("effect"), MaxLength(32)] public int Effect { get; set; }
        [Column("pack_id"), MaxLength(32)] public int PackId { get; set; }
        [Column("animated"), MaxLength(11)] public bool Animated { get; set; }
    }
}