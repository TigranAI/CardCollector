using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Database.Entity;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.InlineQueryResults;

namespace CardCollector.Others
{
    public static class Extensions
    {
        public static void Times(this int count, Action action)
        {
            for (var i = 0; i < count; ++i) action();
        }
        
        public static IEnumerable<InlineQueryResult> ToTelegramStickers(this IEnumerable<Sticker> list, string command,
            int offset)
        {
            var result = new List<InlineQueryResult>();
            foreach (var sticker in list.Skip(offset).Take(50))
                result.Add(new InlineQueryResultCachedSticker($"{command}={sticker.Id}", sticker.FileId));
            return result;
        }

        public static IEnumerable<InlineQueryResult> ToTelegramResults(this IEnumerable<UserSticker> list,
            string command, int offset)
        {
            var result = new List<InlineQueryResult>();
            foreach (var userSticker in list.Skip(offset).Take(50))
                result.Add(new InlineQueryResultArticle(
                    $"{command}={userSticker.Id}",
                    userSticker.Sticker.Title,
                    new InputTextMessageContent(
                        $"{userSticker.User.Username} {Text.bet} " +
                        $"{userSticker.Sticker.Title} {userSticker.Sticker.TierAsStars()}"))
                {
                    Description =
                        $"{Text.tier}: {userSticker.Sticker.TierAsStars()} | {Text.count}: {userSticker.Count}"
                });
            return result;
        }

        public static IEnumerable<InlineQueryResult> ToTelegramStickersAsMessage(this IEnumerable<Sticker> list,
            string command, int offset)
        {
            var result = new List<InlineQueryResult>();
            foreach (var sticker in list.Skip(offset).Take(50))
                result.Add(new InlineQueryResultCachedSticker($"{command}={sticker.Id}", sticker.FileId)
                    {InputMessageContent = new InputTextMessageContent(Text.select)});
            return result;
        }

        public static IEnumerable<InlineQueryResult> ToTelegramResults
            (this IEnumerable<Auction> list, string command, int offset, double discount)
        {
            var result = new List<InlineQueryResult>();
            foreach (var item in list.Skip(offset).Take(50))
            {
                var price = (int) (item.Price * discount);
                result.Add(new InlineQueryResultArticle($"{command}={item.Id}",
                        $"{item.Trader.Username} {item.Count}{Text.items}", new InputTextMessageContent(Text.buy))
                    {Description = $"{price}{Text.gem} {Text.per} 1{Text.items}"});
            }

            return result;
        }

        public static IEnumerable<InlineQueryResult> ToTelegramResults
            (this IEnumerable<TelegramChat> list, string command, int offset)
        {
            var result = new List<InlineQueryResult>();
            foreach (var item in list.Skip(offset).Take(50))
            {
                result.Add(new InlineQueryResultArticle(
                    $"{command}={item.Id}",
                    item.Title ?? item.ChatId.ToString(),
                    new InputTextMessageContent(Text.select))
                );
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
                if (await predicate(element))
                    return true;
            return false;
        }

        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
        {
            return source.Select((item, index) => (item, index));
        }

        public static async Task<int> SumAsync<TSource>(this IEnumerable<TSource> source,
            Func<TSource, Task<int>> selector)
        {
            var sum = 0;
            foreach (var item in source) sum += await selector(item);
            return sum;
        }
    }
}