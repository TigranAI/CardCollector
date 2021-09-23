using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.DataBase.Entity
{
    /* Объект таблицы cash (одна строка)
     В таблице хранится Id пользователя, количество монет и алмазов*/
    [Table("cash")]
    public class CashEntity
    {
        /* Id пользователя */
        [Key]
        [Column("user_id"), MaxLength(127)] public long UserId { get; set; }
        
        /* Количество монет */
        [Column("coins"), MaxLength(32)] public int Coins { get; set; } = 10000;
        
        /* Количество алмазов */
        [Column("gems"), MaxLength(32)] public int Gems { get; set; } = 1000;
    }
}