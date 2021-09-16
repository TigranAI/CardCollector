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
        public async Task<IEnumerable<InlineQueryResult>> GetStickersList(string command, string filter, bool userFilterEnabled = false)
        {
            /* Получаем список стикеров исходя из того, нужно ли для отладки получить бесконечные стикеры */
            var stickersList = Constants.UNLIMITED_ALL_STICKERS
                ? await GetUnlimitedStickers(filter) : await GetUserStickers(filter);
            /* Если пользовательская сортировка не применена, то возвращаем реультат */
            if (!userFilterEnabled) return ToTelegramResults(stickersList, command);
            /* Фильтруем по автору */
            if (Filters["author"] is not "")
                stickersList = stickersList.Where(item => item.Author.Contains((string) Filters["author"]));
            /* Фильтруем по тиру */
            if (Filters["tier"] is not -1)
                stickersList = stickersList.Where(item => item.Tier.Equals((int) Filters["tier"]));
            /* Фильтруем по эмоции */
            if (Filters["emoji"] is not "")
                stickersList = stickersList.Where(item => item.Emoji.Contains((string) Filters["emoji"]));
            /* Фильтруем по цене если пользователь не в меню коллекции */
            if (Filters["emoji"] is not "" && State is not UserState.CollectionMenu)
                stickersList = stickersList.Where(item => item.Emoji.Contains((string) Filters["emoji"]));
            /* Если не установлена сортировка, возвращаем результат */
            if ((string) Filters["sorting"] == SortingTypes.None) return ToTelegramResults(stickersList, command);
            
            /* Сортируем список, если тип сортировки установлен */
            /* Сортируем по автору */
            if ((string) Filters["sorting"] == SortingTypes.ByAuthor)
                stickersList = stickersList.OrderBy(item => item.Author);
            /* Сортируем по названию */
            if ((string) Filters["sorting"] == SortingTypes.ByTitle)
                stickersList = stickersList.OrderBy(item => item.Title);
            /* Сортируем по увеличению тира */
            if ((string) Filters["sorting"] == SortingTypes.ByTierIncrease)
                stickersList = stickersList.OrderBy(item => item.Tier);
            /* Сортируем по уменьшению тира */
            if ((string) Filters["sorting"] == SortingTypes.ByTierDecrease)
                stickersList = stickersList.OrderByDescending(item => item.Tier);
            
            return ToTelegramResults(stickersList, command);
        }

        /* Возвращает все стикеры системы */
        private static async Task<IEnumerable<StickerEntity>> GetUnlimitedStickers(string filter)
        {
            return (await StickerDao.GetAll())
                .Where(item => item.Title.Contains(filter));
        }

        /* Возвращает все стикеры системы */
        private async Task<IEnumerable<StickerEntity>> GetUserStickers(string filter)
        {
            var result = new List<StickerEntity>();
            foreach (var relation in Stickers.Values.Where(i => i.Count > 0))
            {
                var sticker = await StickerDao.GetStickerByHash(relation.StickerId);
                if (sticker.Title.Contains(filter)) result.Add(sticker);
            }
            return result;
        }

        /* Преобразует список стикеров в список результатов для телеграм */
        private static IEnumerable<InlineQueryResult> ToTelegramResults(IEnumerable<StickerEntity> list, string command)
        {
            var result = new List<InlineQueryResult>();
            foreach (var item in list)
            {
                result.Add(new 
                    InlineQueryResultCachedSticker(
                        $"{(Constants.UNLIMITED_ALL_STICKERS ? InlineQueryCommands.unlimited_stickers : "")}{command}={item.Md5Hash}",
                        item.Id));
                /* Ограничение Telegram API по количеству результатов в 50 шт. */
                if (result.Count > 49) return result;
            }
            return result;
        }
    }
}