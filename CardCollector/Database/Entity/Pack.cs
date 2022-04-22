using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.Database.Entity
{
    public class Pack
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(127)] public string Author { get; set; }
        [MaxLength(512)] public string? Description { get; set; }
        public int PriceCoins { get; set; }
        public int PriceGems { get; set; }
        [MaxLength(127)] public string? PreviewFileId { get; set; }
        public bool? IsPreviewAnimated { get; set; }
        public string? GifPreviewFileId { get; set; }
        public long OpenedCount { get; set; }
        public virtual ICollection<Sticker> Stickers { get; set; }
    }
}