using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.DataBase.Entity
{
    [Table("stickers")]
    public class StickerEntity
    {
        [Column("id"), MaxLength(127)] public string Id { get; set; }
        [Column("title"), MaxLength(256)] public string Title { get; set; }
        [Column("author"), MaxLength(128)] public string Author { get; set; }

        [Column("income_coins"), MaxLength(32)] public int IncomeCoins { get; set; } = 0;
        [Column("income_gems"), MaxLength(32)] public int IncomeGems { get; set; } = 0;
        [Column("tier"), MaxLength(32)] public int Tier { get; set; } = 1;
        [Column("emoji"), MaxLength(127)] public string Emoji { get; set; } = "";
        [Column("description"), MaxLength(1024)] public string Description { get; set; } = "";
    }
}