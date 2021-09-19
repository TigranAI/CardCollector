using System;
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
            {Command.author, ""},
            {Command.tier, -1},
            {Command.emoji, ""},
            {Command.price_coins_from, 0},
            {Command.price_coins_to, 0},
            {Command.price_gems_from, 0},
            {Command.price_gems_to, 0},
            {Command.sort, SortingTypes.None},
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
                ? await GetUnlimitedStickers(filter)
                : State switch
                {
                    UserState.CollectionMenu => await GetUserStickers(filter),
                    UserState.AuctionMenu => await GetAuctionStickers(filter),
                    UserState.ShopMenu => await GetShopStickers(filter),
                    _ => await GetUserStickers(filter)
                };
            /* Если пользовательская сортировка не применена, то возвращаем реультат */
            if (!userFilterEnabled || State == UserState.Default) return ToTelegramResults(stickersList, command);
            /* Фильтруем по автору */
            if (Filters[Command.author] is not "")
                stickersList = stickersList.Where(item => item.Author.Contains((string) Filters[Command.author]));
            /* Фильтруем по тиру */
            if (Filters[Command.tier] is not -1)
                stickersList = stickersList.Where(item => item.Tier.Equals((int) Filters[Command.tier]));
            /* Фильтруем по эмоции */
            if (Filters[Command.emoji] is not "")
                stickersList = stickersList.Where(item => item.Emoji.Contains((string) Filters[Command.emoji]));
            /* Фильтруем по цене монет ОТ если пользователь не в меню коллекции */
            if (Filters[Command.price_coins_from] is not 0 && State is not UserState.CollectionMenu)
                stickersList = stickersList.Where(item => item.PriceCoins >= (int)Filters[Command.price_coins_from]);
            /* Фильтруем по цене монет ДО если пользователь не в меню коллекции */
            if (Filters[Command.price_coins_to] is not 0 && State is not UserState.CollectionMenu)
                stickersList = stickersList.Where(item => item.PriceCoins <= (int)Filters[Command.price_coins_to]);
            /* Фильтруем по цене алмазов ОТ если пользователь не в меню коллекции */
            if (Filters[Command.price_gems_from] is not 0 && State is not UserState.CollectionMenu)
                stickersList = stickersList.Where(item => item.PriceGems >= (int)Filters[Command.price_gems_from]);
            /* Фильтруем по цене адмазов ДО если пользователь не в меню коллекции */
            if (Filters[Command.price_gems_to] is not 0 && State is not UserState.CollectionMenu)
                stickersList = stickersList.Where(item => item.PriceGems <= (int)Filters[Command.price_gems_to]);
            /* Если не установлена сортировка, возвращаем результат */
            if ((string) Filters[Command.sort] == SortingTypes.None) return ToTelegramResults(stickersList, command);
            
            /* Сортируем список, если тип сортировки установлен */
            /* Сортируем по автору */
            if ((string) Filters[Command.sort] == SortingTypes.ByAuthor)
                stickersList = stickersList.OrderBy(item => item.Author);
            /* Сортируем по названию */
            if ((string) Filters[Command.sort] == SortingTypes.ByTitle)
                stickersList = stickersList.OrderBy(item => item.Title);
            /* Сортируем по увеличению тира */
            if ((string) Filters[Command.sort] == SortingTypes.ByTierIncrease)
                stickersList = stickersList.OrderBy(item => item.Tier);
            /* Сортируем по уменьшению тира */
            if ((string) Filters[Command.sort] == SortingTypes.ByTierDecrease)
                stickersList = stickersList.OrderByDescending(item => item.Tier);
            
            return ToTelegramResults(stickersList, command);
        }

        private static async Task<IEnumerable<StickerEntity>> GetShopStickers(string filter)
        {
            // TODO return shop stickers
            return await GetUnlimitedStickers(filter);
        }

        private static async Task<IEnumerable<StickerEntity>> GetAuctionStickers(string filter)
        {
            // TODO return auction stickers
            return await GetUnlimitedStickers(filter);
        }

        /* Возвращает все стикеры системы */
        private static async Task<IEnumerable<StickerEntity>> GetUnlimitedStickers(string filter)
        {
            return (await StickerDao.GetAll())
                .Where(item => item.Title.Contains(filter, StringComparison.OrdinalIgnoreCase));
        }

        /* Возвращает все стикеры системы */
        private async Task<IEnumerable<StickerEntity>> GetUserStickers(string filter)
        {
            var result = new List<StickerEntity>();
            foreach (var relation in Stickers.Values.Where(i => i.Count > 0))
            {
                var sticker = await StickerDao.GetStickerByHash(relation.StickerId);
                if (sticker.Title.Contains(filter, StringComparison.OrdinalIgnoreCase)) result.Add(sticker);
            }
            return result;
        }

        /* Преобразует список стикеров в список результатов для телеграм */
        private static IEnumerable<InlineQueryResult> ToTelegramResults(IEnumerable<StickerEntity> list, string command)
        {
            var result = new List<InlineQueryResult>();
            foreach (var item in list)
            {
                result.Add(new InlineQueryResultCachedSticker(
                        $"{(Constants.UNLIMITED_ALL_STICKERS ? Command.unlimited_stickers : "")}{command}={item.Md5Hash}",
                        item.Id));
                /* Ограничение Telegram API по количеству результатов в 50 шт. */
                if (result.Count > 49) return result;
            }
            return result;
        }
    }
}