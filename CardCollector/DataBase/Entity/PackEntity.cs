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
    }
}