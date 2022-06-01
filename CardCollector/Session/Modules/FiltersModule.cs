using System.Collections.Generic;
using System.Linq;
using CardCollector.Database.Entity;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.Resources.Translations.Providers;
using SortingTypes = CardCollector.Resources.Enums.SortingTypes;

namespace CardCollector.Session.Modules
{
    public class FiltersModule : Module
    {
        public string? Author;
        public int? Tier;
        public string? Emoji;
        public int? PriceGemsFrom;
        public int? PriceGemsTo;
        public SortingTypes Sorting;

        public string ToString(UserState state)
        {
            var text = $"{Messages.current_filters}:\n" +
                       $"{Messages.author}: {Author ?? Messages.all}\n" +
                       $"{Messages.tier}: {Tier?.ToString() ?? Messages.all}\n" +
                       $"{Messages.emoji}: {Emoji ?? Messages.all}\n";
            if (state is UserState.AuctionMenu)
                text += $"{Messages.price}: 💎 {PriceGemsFrom ?? 0} - {PriceGemsTo?.ToString() ?? "∞"}\n";

            text += $"{Messages.sorting} {SortingTypesProvider.Instance[Sorting]}" +
                    $"\n\n{Messages.select_filter}:";
            return text;
        }

        public IEnumerable<Sticker> ApplyTo(IEnumerable<Sticker> list)
        {
            if (Author is { } author)
                list = list.Where(item => item.Author.Contains(author));
            if (Tier is { } tier)
                list = list.Where(item => item.Tier.Equals(tier));
            if (Emoji is { } emoji)
                list = list.Where(item => item.Emoji.Contains(emoji));
            return Sorting switch
            {
                SortingTypes.ByAuthor => list.OrderBy(item => item.Author).ToList(),
                SortingTypes.ByTitle => list.OrderBy(item => item.Title).ToList(),
                SortingTypes.ByTierIncrease => list.OrderBy(item => item.Tier).ToList(),
                SortingTypes.ByTierDecrease => list.OrderByDescending(item => item.Tier).ToList(),
                _ => list
            };
        }

        public IEnumerable<UserSticker> ApplyTo(IEnumerable<UserSticker> list)
        {
            if (Author is { } author)
                list = list.Where(item => item.Sticker.Author.Contains(author));
            if (Tier is { } tier)
                list = list.Where(item => item.Sticker.Tier.Equals(tier));
            if (Emoji is { } emoji)
                list = list.Where(item => item.Sticker.Emoji.Contains(emoji));
            return Sorting switch
            {
                SortingTypes.ByAuthor => list.OrderBy(item => item.Sticker.Author).ToList(),
                SortingTypes.ByTitle => list.OrderBy(item => item.Sticker.Title).ToList(),
                SortingTypes.ByTierIncrease => list.OrderBy(item => item.Sticker.Tier).ToList(),
                SortingTypes.ByTierDecrease => list.OrderByDescending(item => item.Sticker.Tier).ToList(),
                _ => list
            };
        }

        public IEnumerable<Auction> ApplyPriceTo(IEnumerable<Auction> list)
        {
            if (PriceGemsFrom is { } && PriceGemsTo is { })
                return list.Where(item => item.Price >= PriceGemsFrom && item.Price <= PriceGemsTo);
            if (PriceGemsFrom is { })
                return list.Where(item => item.Price >= PriceGemsFrom);
            if (PriceGemsTo is { })
                return list.Where(item => item.Price <= PriceGemsTo);
            return list;
        }

        public void Reset()
        {
            Author = null;
            Tier = null;
            Emoji = null;
            PriceGemsFrom = null;
            PriceGemsTo = null;
            Sorting = SortingTypes.None;
        }
    }
}