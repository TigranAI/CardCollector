using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
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

        /* Уровень привилегий пользователя */
        [Column("privilege_level"), MaxLength(32)] public int PrivilegeLevel { get; set; } = 0;
        
        [Column("current_profile_massage_id")] public int CurrentProfileMessageId { get; set; }
        
        /* Счет пользователя */
        [NotMapped] public CashEntity Cash { get; set; }
        
        /* Стикеры пользователя */
        [NotMapped] public Dictionary<string, UserStickerRelationEntity> Stickers { get; set; }

        /* Текущее состояние пользователя */
        [NotMapped] public UserState State = UserState.Default;

        /* Фильтры, примененные пользователем в меню коллекции/магазина/аукциона */
        [NotMapped] public readonly Dictionary<string, object> Filters = new()
        {
            {"author", ""},
            {"tier", -1},
            {"emoji", ""},
            {"sorting", SortingTypes.None},
            {"price", 0},
        };

        /* Сообщения в чате пользователя */
        [NotMapped] public readonly List<int> Messages = new();

        /* Удаляет из чата все сообщения, добавленные в список выше */
        public async Task ClearChat()
        {
            foreach (var messageId in Messages)
                await MessageController.DeleteMessage(this, messageId);
            Messages.Clear();
        }
        
        /* Возвращает стикеры в виде объектов телеграм */
        public async Task<IEnumerable<InlineQueryResult>> GetStickersList(string command, string filter)
        {
            var result = new List<InlineQueryResult>();
            if (Constants.UNLIMITED_ALL_STICKERS)
                foreach (var sticker in await StickerDao.GetAll())
                {
                    var item = new InlineQueryResultCachedSticker($"unlimited_sticker={sticker.Title}", sticker.Id);
                    result.Add(item);
                    if (result.Count > 49) return result;
                }
            else
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