using System;
using System.Collections.Generic;
using System.Linq;
using CardCollector.Database.Entity;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;
using Telegram.Bot.Types.InlineQueryResults;

namespace CardCollector.Others
{
    public static class Extensions
    {
        public static void Times(this int count, Action action)
        {
            for (var i = 0; i < count; ++i) action();
        }
        
        public static IEnumerable<InlineQueryResult> ToTelegramResults<T>(
            this IEnumerable<T> source,
            string command,
            Offset offset) where T : ITelegramInlineQueryResult
        {
            return source
                .ApplyOffset(offset)
                .Select(item => item.ToResult(command));
        }

        public static IEnumerable<InlineQueryResult> ToTelegramMessageResults<T>(
            this IEnumerable<T> list,
            string command,
            Offset offset) where T : ITelegramInlineQueryMessageResult
        {
            return list
                .ApplyOffset(offset)
                .Select(sticker =>
                {
                    var result = (InlineQueryResultCachedSticker) sticker.ToMessageResult(command);
                    result.InputMessageContent = new InputTextMessageContent(Text.select);
                    return result;
                });
        }

        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
        {
            return source.Select((item, index) => (item, index));
        }

        public static IEnumerable<T> ApplyOffset<T>(this IEnumerable<T> source, Offset offset)
        {
            return source
                .Skip(offset.Value)
                .Take(50);
        }

        public static IEnumerable<Sticker> ApplyFilters(this IEnumerable<Sticker> source, FiltersModule filters)
        {
            return filters.ApplyTo(source);
        }

        public static IEnumerable<UserSticker> ApplyFilters(this IEnumerable<UserSticker> source, FiltersModule filters)
        {
            return filters.ApplyTo(source);
        }
    }
}