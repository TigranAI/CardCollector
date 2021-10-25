using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session;
using CardCollector.StickerEffects;

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
        [Column("username"), MaxLength(256)] public string Username { get; set; } = "";
        
        /* Заблокирован ли пользователь */
        [Column("is_blocked"), MaxLength(11)] public bool IsBlocked { get; set; } = false;

        /* Уровень привилегий пользователя */
        [Column("privilege_level"), MaxLength(32)] public int PrivilegeLevel { get; set; } = 0;
        
        /* Счет пользователя */
        [NotMapped] public CashEntity Cash { get; set; }
        
        /* Стикеры пользователя */
        [NotMapped] public Dictionary<string, UserStickerRelationEntity> Stickers { get; set; }
        
        /* Данные, хранящиеся в рамках одной сессии */
        [NotMapped] public UserSession Session;

        /* Удаляет из чата все сообщения, добавленные в список выше */
        public async Task ClearChat()
        {
            await Session.ClearMessages();
        }
        
        /* Возвращает стикеры пользователя */
        public async Task<IEnumerable<StickerEntity>> GetStickersList(string filter, int tier = -1)
        {
            if (Constants.UNLIMITED_ALL_STICKERS) return await StickerDao.GetAll(filter);
            var relationStickers = Stickers.Values.Where(relation => relation.Count > 0);
            var stickerList = await Task.WhenAll(relationStickers
                .Select(async rel => await StickerDao.GetByHash(rel.ShortHash)));
            return stickerList.Where(sticker => sticker.Contains(filter) && (tier == -1 || sticker.Tier == tier));
        }

        public async Task<int> AuctionDiscount()
        {
            return await AuctionDiscount5.IsApplied(Stickers) ? 5 : 0;
        }

        public UserEntity()
        {
            Session = new UserSession(this);
        }
    }
}