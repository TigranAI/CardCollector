using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
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
        public static async Task<IEnumerable<InlineQueryResult>> ToTelegramResults
            (this IEnumerable<AuctionEntity> list, string command, double discount)
        {
            var result = new List<InlineQueryResult>();
            foreach (var item in list)
            {
                var price = (int)(item.Price * discount);
                var user = await UserDao.GetById(item.Trader);
                result.Add(new InlineQueryResultArticle($"{command}={item.Id}",
                    $"{user.Username} {item.Count}{Text.items}", new InputTextMessageContent(Text.buy))
                { Description = $"{price}{Text.gem} {Text.per} 1{Text.items}" });
                /* Ограничение Telegram API по количеству результатов в 50 шт. */
                if (result.Count > 49) return result;
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

        public static async Task<bool> AnyAsync<T>(
            this IEnumerable<T> source, Func<T, Task<bool>> predicate)
        {
            foreach (var element in source)
                if (await predicate(element)) return true;
            return false;
        }
        
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
        {
            return source.Select((item, index) => (item, index));
        }
        
        public static async Task<int> SumAsync<TSource>(this IEnumerable<TSource> source, Func<TSource, Task<int>> selector)
        {
            var sum = 0;
            foreach (var item in source) sum += await selector(item);
            return sum;
        }
    }
}