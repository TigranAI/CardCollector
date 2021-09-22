using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.EntityDao;
using CardCollector.Others;
using CardCollector.Resources;
using Telegram.Bot.Types.InlineQueryResults;

namespace CardCollector.DataBase.Entity
{
    /* Этот класс представляет собой строку таблицы users и описывает объект пользователя */
    [Table("users")]
    public partial class UserEntity
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

        /* Уровень привилегий пользователя */
        [Column("privilege_level"), MaxLength(32)] public int PrivilegeLevel { get; set; } = 0;
        
        /* Счет пользователя */
        [NotMapped] public CashEntity Cash { get; set; }
        
        /* Стикеры пользователя */
        [NotMapped] public Dictionary<string, UserStickerRelationEntity> Stickers { get; set; }
        
        /* Данные, хранящиеся в рамках одной сессии */
        [NotMapped] public readonly UserSession Session;

        /* Удаляет из чата все сообщения, добавленные в список выше */
        public async Task ClearChat()
        {
            await Session.ClearMessages();
        }
        
        /* Возвращает стикеры в виде объектов телеграм */
        public async Task<IEnumerable<InlineQueryResult>> GetStickersList(string command, string filter, bool userFilterEnabled = false)
        {
            var state = Session.State;
            /* Получаем список стикеров исходя из того, нужно ли для отладки получить бесконечные стикеры */
            var stickersList = state switch
                {
                    UserState.AuctionMenu => await AuctionController.GetStickers(filter),
                    UserState.ShopMenu => await ShopController.GetStickers(filter),
                    _ => Constants.UNLIMITED_ALL_STICKERS 
                        ? await StickerDao.GetAll(filter)
                        : await Stickers.ToStickers(filter),
                };
            /* Если пользовательская сортировка не применена, то возвращаем реультат */
            if (!userFilterEnabled || state == UserState.Default) 
                return stickersList.ToTelegramResults(command);
            return Session.Filters.ApplyTo(stickersList, state).ToTelegramResults(command);
        }

        public UserEntity()
        {
            Session = new UserSession(this);
        }
    }
}