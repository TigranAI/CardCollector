using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.Database.Entity
{
    public class PuzzlePiece
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public virtual Sticker Sticker { get; set; }
        [MaxLength(127)] public string FileId { get; set; }
        public int Order { get; set; }
        public int PieceCount { get; set; }
    }
}