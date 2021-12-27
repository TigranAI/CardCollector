using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.DataBase.Entity
{
    [Table("packs")]
    public class PackEntity
    {
        /* Id набора */
        [Column("id"), MaxLength(32)] public int Id { get; set; }
        
        /* Имя автора */
        [Column("author"), MaxLength(127)] public string Author { get; set; }
        
        /* Описание */
        [Column("description"), MaxLength(512)] public string Description { get; set; }
        
        /* Превьюшка пака */
        [Column("sticker_preview"), MaxLength(127)] public string StickerPreview { get; set; }
        [Column("animated"), MaxLength(11)] public bool Animated { get; set; }
        
        /* Количество открытий */
        [Column("opened_count"), MaxLength(127)] public long OpenedCount { get; set; } = 0;

        [NotMapped] public int PriceCoins => Id == 1 ? 1000 : -1;
        [NotMapped] public int PriceGems => 100;
    }
}