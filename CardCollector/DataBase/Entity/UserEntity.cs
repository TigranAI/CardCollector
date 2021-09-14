using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.EntityDao;
using Telegram.Bot.Types.InlineQueryResults;

namespace CardCollector.DataBase.Entity
{
    /* Этот класс представляет собой строку таблицы users и описывает объект пользователя */
    [Table("users")]
    public class UserEntity
    {
        /* Id пользователя */
        [Key]
        [Column("id"), MaxLength(127)] public long Id { get; set; }
        
        /* Id чата */
        [Column("chat_id"), MaxLength(127)] public long ChatId { get; set; }
        
        /* Имя пользователя */
        [Column("username"), MaxLength(256)] public string Username { get; set; }
        
        /* Заблокирован ли пользователь */
        [Column("is_blocked"), MaxLength(11)] public bool IsBlocked { get; set; }
        
        /* Счет пользователя */
        [NotMapped] public CashEntity Cash { get; set; }
        
        /* Стикеры пользователя */
        [NotMapped] public Dictionary<string, UserStickerRelationEntity> Stickers { get; set; }
        
        /* Возвращает стикеры в виде объектов телеграм */
        public async Task<IEnumerable<InlineQueryResult>> GetStickersList(string command, string filter)
        {
            var result = new List<InlineQueryResult>();
            foreach (var sticker in Stickers.Values.Where(i => i.Count > 0))
            {
                if (filter != "")
                {
                    var stickerInfo = await StickerDao.GetStickerInfo(sticker.StickerId);
                    if (!stickerInfo.Title.Contains(filter)) break;
                }
                var item = new InlineQueryResultCachedSticker($"{command}={sticker.ShortHash}", sticker.StickerId);
                result.Add(item);
                if (result.Count > 49) return result;
            }
            return result;
        }
    }
}