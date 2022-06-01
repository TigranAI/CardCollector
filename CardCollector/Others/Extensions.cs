using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardCollector.Database.Entity;
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
                .Select(sticker => sticker.ToMessageResult(command));
        }

        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
        {
            return source.Select((item, index) => (item, index));
        }

        public static IEnumerable<T> ApplyOffset<T>(this IEnumerable<T> source, Offset offset)
        {
            return source
                .Skip(offset.Value)
                .Take(offset.Step);
        }

        public static IEnumerable<Sticker> ApplyFilters(this IEnumerable<Sticker> source, FiltersModule filters)
        {
            return filters.ApplyTo(source);
        }

        public static IEnumerable<UserSticker> ApplyFilters(this IEnumerable<UserSticker> source, FiltersModule filters)
        {
            return filters.ApplyTo(source);
        }

        public static IEnumerable<Sticker> ApplyFilters(this IEnumerable<Auction> source, FiltersModule filters)
        {
            return source
                .ApplyFiltersPrice(filters)
                .Select(item => item.Sticker)
                .ApplyFilters(filters);
        }

        public static IEnumerable<Auction> ApplyFiltersPrice(this IEnumerable<Auction> source, FiltersModule filters)
        {
            return filters.ApplyPriceTo(source);
        }
    }
}