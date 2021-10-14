using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.DataBase.Entity
{
    [Table("users_packs")]
    public class UsersPacksEntity
    {
        /* Id записи в таблице, роли не играет */
        [Column("id"), MaxLength(127)] public long Id { get; set; }
        
        /* Id комплекта */
        [Column("pack_id"), MaxLength(32)] public int PackId { get; set; }
        
        /* Id пользователя */
        [Column("user_id"), MaxLength(127)] public long UserId { get; set; }
        
        /* Количество паков у пользователя */
        [Column("count"), MaxLength(32)] public int Count { get; set; }
    }
}