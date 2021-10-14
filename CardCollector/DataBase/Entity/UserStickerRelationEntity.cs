using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using CardCollector.DataBase.EntityDao;

namespace CardCollector.DataBase.Entity
{
    /* Явялется расширенной связью многий ко многим между таблицами пользователей и стикеров */
    [Table("user_to_stickers_relations")]
    public class UserStickerRelationEntity
    {
        /* Id записи в таблице, роли не играет */
        [Column("id"), MaxLength(127)] public long Id { get; set; }
        
        /* Id стикера на серверах Телеграм */
        [Column("sticker_id"), MaxLength(127)] public string StickerId { get; set; }
        
        /* Id пользователя */
        [Column("user_id"), MaxLength(127)] public long UserId { get; set; }
        
        /* Количество стикеров данного вида у пользователя */
        [Column("count"), MaxLength(32)] public int Count { get; set; }
        
        /* Последняя выплата по данному стикеру */
        [Column("payout"), MaxLength(32)] public DateTime Payout { get; set; } = DateTime.Now;
        
        /* MD5 хеш id стикера */
        [Column("short_hash"), MaxLength(40)] public string ShortHash { get; set; }
        
        /* дополнительные данные */
        [Column("additional_data"), MaxLength(512)] public string AdditionalData { get; set; } = "";
    }
}