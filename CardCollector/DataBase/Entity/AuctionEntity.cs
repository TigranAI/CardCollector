using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace CardCollector.DataBase.Entity
{
    [Table("auction")]
    public class AuctionEntity
    {
        /* id стикера */
        [Column("id"), MaxLength(127)] public long Id { get; set; }
        
        /* цена  */
        [Column("price"), MaxLength(32)] public int Price { get; set; }
        
        /* валюта */
        [Column("valuta"), MaxLength(16)] public string Valuta { get; set; }
        
        /* количество */
        [Column("quantity"), MaxLength(32)] public int Quantity { get; set; }
        
        /* продавец */
        [Column("trader"), MaxLength(256)] public string Trader { get; set; }
        
    }
}