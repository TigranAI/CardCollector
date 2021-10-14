using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Microsoft.EntityFrameworkCore;
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
        public static async Task<IEnumerable<InlineQueryResult>> ToTelegramResults
            (this IEnumerable<AuctionEntity> list, string command)
        {
            var result = new List<InlineQueryResult>();
            foreach (var item in list)
            {
                var user = await UserDao.GetById(item.Trader);
                result.Add(new InlineQueryResultArticle($"{command}={item.Id}",
                    $"{user.Username} {item.Count}{Text.items}", new InputTextMessageContent(Text.buy))
                { Description = $"{item.Price}{Text.gem} {Text.per} 1{Text.items}" });
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

        public static async Task<IEnumerable<T>> WhereAsync<T>(
            this IEnumerable<T> source, Func<T, Task<bool>> predicate)
        {
            var results = new ConcurrentQueue<T>();
            var tasks = source.Select(
                async x =>
                {
                    if (await predicate(x))
                        results.Enqueue(x);
                });
            await Task.WhenAll(tasks);
            return results;
        }

        public static async Task<IEnumerable<TSource>> WhereAsync<TSource>(
            [NotNull] this IQueryable<TSource> source, 
            [NotNull] Func<TSource, bool> predicate,
            CancellationToken cancellationToken = default)
        {
            var results = new ConcurrentQueue<TSource>();
            await foreach (var element in source.AsAsyncEnumerable().WithCancellation(cancellationToken))
            {
                if (predicate(element)) results.Enqueue(element);
            }
            return results;
        }
        
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
        {
            return source.Select((item, index) => (item, index));
        }
        
        public static async Task<int> SumAsync<TSource>(this IEnumerable<TSource> source, Func<TSource, Task<int>> selector)
        {
            var sum = 0;
            checked
            {
                foreach (var item in source) sum += await selector(item);
            }
            return sum;
        }
    }
}