using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.Migrations.Database_old.Entity
{
    [Table("packs")]
    public class PackEntity
    {
        [Column("id"), MaxLength(32)] public int Id { get; set; }
        [Column("author"), MaxLength(127)] public string Author { get; set; }
        [Column("description"), MaxLength(512)] public string? Description { get; set; }

        [Column("sticker_preview"), MaxLength(127)] public string? StickerPreview { get; set; }
        [Column("animated"), MaxLength(11)] public bool Animated { get; set; }
        [Column("opened_count"), MaxLength(127)] public long OpenedCount { get; set; }
    }
}