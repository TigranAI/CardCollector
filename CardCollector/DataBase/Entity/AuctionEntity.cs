using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.DataBase.Entity
{
    [Table("auction")]
    public class AuctionEntity
    {
        /* id стикера */
        [Key]
        [Column("id"), MaxLength(32)] public int Id { get; set; }
        
        /* цена */
        [Column("price"), MaxLength(32)] public int Price { get; set; }
        
        /* валюта */
        [Column("currency"), MaxLength(32)] public int Сurrency { get; set; }
        
        /* количество */
        [Column("quantity"), MaxLength(32)] public int Quantity { get; set; }
        
    }
}