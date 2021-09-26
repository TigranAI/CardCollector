using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Others;
using CardCollector.Resources;
using Telegram.Bot.Types.InlineQueryResults;

namespace CardCollector
{
    public static class Extensions
    {
        /* Преобразует список стикеров в список результатов для телеграм */
        public static IEnumerable<InlineQueryResult> ToTelegramResults
            (this IEnumerable<StickerEntity> list, string command, bool asMessage = true)
        {
            var result = new List<InlineQueryResult>();
            foreach (var item in list)
            {
                result.Add( asMessage 
                    ? new InlineQueryResultCachedSticker($"{(Constants.UNLIMITED_ALL_STICKERS ? Command.unlimited_stickers : "")}" +
                                                         $"{command}={item.Md5Hash}", item.Id) 
                                                        {InputMessageContent = new InputTextMessageContent(Text.select)}
                    : new InlineQueryResultCachedSticker($"{(Constants.UNLIMITED_ALL_STICKERS ? Command.unlimited_stickers : "")}" +
                                                         $"{command}={item.Md5Hash}", item.Id));
                /* Ограничение Telegram API по количеству результатов в 50 шт. */
                if (result.Count > 49) return result;
            }
            return result;
        }
        /* Преобразует список продавцов в список результатов для телеграм */
        public static IEnumerable<InlineQueryResult> ToTelegramResults
            (this IEnumerable<TraderInformation> list, string command)
        {
            var result = new List<InlineQueryResult>();
            foreach (var item in list)
            {
                result.Add(new InlineQueryResultArticle($"{command}={item.Id}",
                    $"{item.Username} {item.Quantity}{Text.items}", new InputTextMessageContent(Text.buy))
                { Description = $"{item.PriceCoins}{Text.coin}/{item.PriceGems}{Text.gem} {Text.per} 1{Text.items}" });
                /* Ограничение Telegram API по количеству результатов в 50 шт. */
                if (result.Count > 49) return result;
            }
            return result;
        }
        
        /* Возвращает все стикеры системы */
        public static async Task<IEnumerable<StickerEntity>> ToStickers
            (this Dictionary<string, UserStickerRelationEntity> dict, string filter)
        {
            var result = new List<StickerEntity>();
            foreach (var relation in dict.Values.Where(i => i.Count > 0))
            {
                var sticker = await StickerDao.GetStickerByHash(relation.StickerId);
                if (sticker.Title.Contains(filter, StringComparison.CurrentCultureIgnoreCase)) result.Add(sticker);
            }
            return result;
        }

        public static IEnumerable<StickerEntity> ApplyTo(this Dictionary<string, object> dict,
            IEnumerable<StickerEntity> list, UserState state)
        {
            /* Фильтруем по автору */
            if (dict[Command.author] is string author && author != "")
                list = list.Where(item => item.Author.Contains(author));
            /* Фильтруем по тиру */
            if (dict[Command.tier] is int tier && tier != -1)
                list = list.Where(item => item.Tier.Equals(tier));
            /* Фильтруем по эмоции */
            if (dict[Command.emoji] is string emoji && emoji != "")
                list = list.Where(item => item.Emoji.Contains(emoji));
            /* Если пользвователь не находится в меню коллекции, то фильтруем по цене */
            if (state is not UserState.CollectionMenu)
            {
                /* Фильтруем по цене монет ОТ */
                if (dict[Command.price_coins_from] is int PCF && PCF != 0)
                {
                    list = list.Where(item => state == UserState.AuctionMenu
                        ? AuctionDao.HaveAny(item.Id, i => i.PriceCoins >= PCF)
                        : item.PriceCoins >= PCF);
                }
                /* Фильтруем по цене монет ДО */
                if (dict[Command.price_coins_to] is int PCT && PCT != 0)
                    list = list.Where(item => state == UserState.AuctionMenu
                        ? AuctionDao.HaveAny(item.Id, i => i.PriceCoins <= PCT)
                        : item.PriceCoins <= PCT);
                /* Фильтруем по цене алмазов ОТ */
                if (dict[Command.price_gems_from] is int PGF && PGF != 0)
                    list = list.Where(item => state == UserState.AuctionMenu
                        ? AuctionDao.HaveAny(item.Id, i => i.PriceGems >= PGF)
                        : item.PriceGems >= PGF);
                /* Фильтруем по цене адмазов ДО */
                if (dict[Command.price_gems_to] is int PGT && PGT != 0)
                    list = list.Where(item => state == UserState.AuctionMenu
                        ? AuctionDao.HaveAny(item.Id, i => i.PriceGems <= PGT)
                        : item.PriceGems <= PGT);
            }
            /* Сортируем список, если тип сортировки установлен */
            if (dict[Command.sort] is not string sort || sort == SortingTypes.None) return list;
            {
                /* Сортируем по автору */
                if (sort== SortingTypes.ByAuthor)
                    list = list.OrderBy(item => item.Author);
                /* Сортируем по названию */
                if (sort == SortingTypes.ByTitle)
                    list = list.OrderBy(item => item.Title);
                /* Сортируем по увеличению тира */
                if (sort == SortingTypes.ByTierIncrease)
                    list = list.OrderBy(item => item.Tier);
                /* Сортируем по уменьшению тира */
                if (sort == SortingTypes.ByTierDecrease)
                    list = list.OrderByDescending(item => item.Tier);
            }
            return list;
        }

        public static IEnumerable<TraderInformation> ApplyTo(this Dictionary<string, object> dict, IEnumerable<TraderInformation> list)
        {
            /* Фильтруем по цене монет ОТ */
            if (dict[Command.price_coins_from] is int PCF && PCF != 0)
                list = list.Where(item => item.PriceCoins >= PCF);
            /* Фильтруем по цене монет ДО */
            if (dict[Command.price_coins_to] is int PCT && PCT != 0)
                list = list.Where(item => item.PriceCoins <= PCT);
            /* Фильтруем по цене алмазов ОТ */
            if (dict[Command.price_gems_from] is int PGF && PGF != 0)
                list = list.Where(item => item.PriceGems >= PGF);
            /* Фильтруем по цене адмазов ДО */
            if (dict[Command.price_gems_to] is int PGT && PGT != 0)
                list = list.Where(item => item.PriceGems <= PGT);
            return list;
        }
        
        public static string ToMessage(this Dictionary<string, object> dict, UserState state)
        {
            var text = $"{Messages.current_filters}\n" +
                       $"{Messages.author} {(dict[Command.author] is string author and not "" ? author : Messages.all)}\n" +
                       $"{Messages.tier} {(dict[Command.tier] is int tier and not -1 ? new string('⭐', tier) : Messages.all)}\n" +
                       $"{Messages.emoji} {(dict[Command.emoji] is string emoji and not "" ? emoji : Messages.all)}\n";
            if (state != UserState.CollectionMenu) 
                text += $"{Messages.price} 💰 {dict[Command.price_coins_from]} -" +
                        $" {(dict[Command.price_coins_to] is int c and not 0 ? c : "∞")}\n" +
                        $"{Messages.price} 💎 {dict[Command.price_gems_from]} -" +
                        $" {(dict[Command.price_gems_to] is int g and not 0 ? g : "∞")}\n";
            text += $"{Messages.sorting} {dict[Command.sort]}\n\n{Messages.select_filter}";
            return text;
        }
    }
}