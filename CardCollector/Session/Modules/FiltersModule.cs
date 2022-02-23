using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Database.Entity;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using Microsoft.EntityFrameworkCore;
using SortingTypes = CardCollector.Resources.Enums.SortingTypes;

namespace CardCollector.Session.Modules
{
    public class FiltersModule : Module
    {
        public string? Author;
        public int? Tier;
        public string? Emoji;
        public int? PriceCoinsFrom;
        public int? PriceCoinsTo;
        public int? PriceGemsFrom;
        public int? PriceGemsTo;
        public SortingTypes Sorting;

        public string ToString(UserState state)
        {
            var text = $"{Messages.current_filters}:\n" +
                       $"{Messages.author}: {Author ?? Messages.all}\n" +
                       $"{Messages.tier}: {Tier?.ToString() ?? Messages.all}\n" +
                       $"{Messages.emoji}: {Emoji ?? Messages.all}\n";
            switch (state)
            {
                case UserState.AuctionMenu:
                    text += $"{Messages.price}: 💎 {PriceGemsFrom ?? 0} - {PriceGemsTo?.ToString() ?? "∞"}\n";
                    break;
                case UserState.ShopMenu:
                    text += $"{Messages.price}: 💰 {PriceCoinsFrom ?? 0} - {PriceCoinsTo?.ToString() ?? "∞"}\n";
                    break;
            }

            text += $"{Messages.sorting} {Resources.Translations.SortingTypes.ResourceManager.GetString(Sorting.ToString())}" +
                    $"\n\n{Messages.select_filter}:";
            return text;
        }

        public List<Sticker> ApplyTo(List<Sticker> list)
        {
            if (Author is { } author)
                list.RemoveAll(item => !item.Author.Contains(author));
            if (Tier is { } tier)
                list.RemoveAll(item => !item.Tier.Equals(tier));
            if (Emoji is { } emoji)
                list.RemoveAll(item => !item.Emoji.Contains(emoji));
            return Sorting switch
            {
                SortingTypes.None => list,
                SortingTypes.ByAuthor => list.OrderBy(item => item.Author).ToList(),
                SortingTypes.ByTitle => list.OrderBy(item => item.Title).ToList(),
                SortingTypes.ByTierIncrease => list.OrderBy(item => item.Tier).ToList(),
                SortingTypes.ByTierDecrease => list.OrderByDescending(item => item.Tier).ToList(),
                _ => list
            };
        }

        public async Task ApplyPriceTo(BotDatabaseContext context, List<Sticker> list)
        {
            if (PriceGemsFrom == null && PriceGemsTo == null) return;
            if (PriceGemsFrom is { } && PriceGemsTo is { })
            {
                var stickers = await context.Auctions
                    .Where(item => item.Price >= PriceGemsFrom && item.Price <= PriceGemsTo)
                    .DistinctBy(item => item.Sticker.Id)
                    .ToListAsync();
                list.RemoveAll(item => !stickers.Any(auction => auction.Sticker.Id == item.Id));
            }
            else if (PriceGemsFrom is { })
            {
                var stickers = await context.Auctions
                    .Where(item => item.Price >= PriceGemsFrom)
                    .DistinctBy(item => item.Sticker.Id)
                    .ToListAsync();
                list.RemoveAll(item => !stickers.Any(auction => auction.Sticker.Id == item.Id));
            }
            else if (PriceGemsTo is { })
            {
                var stickers = await context.Auctions
                    .Where(item => item.Price <= PriceGemsTo)
                    .DistinctBy(item => item.Sticker.Id)
                    .ToListAsync();
                list.RemoveAll(item => !stickers.Any(auction => auction.Sticker.Id == item.Id));
            }
        }

        public void ApplyPriceTo(List<Auction> list)
        {
            if (PriceGemsFrom is { } && PriceGemsTo is { })
                list.RemoveAll(item => item.Price <= PriceCoinsFrom || item.Price >= PriceGemsTo);
            else if (PriceGemsFrom is { })
                list.RemoveAll(item => item.Price <= PriceCoinsFrom);
            else if (PriceGemsTo is { })
                list.RemoveAll(item => item.Price >= PriceGemsTo);
        }

        public void Reset()
        {
            Author = null;
            Tier = null;
            Emoji = null;
            PriceCoinsFrom = null;
            PriceCoinsTo = null;
            PriceGemsFrom = null;
            PriceGemsTo = null;
            Sorting = SortingTypes.None;
        }
    }
}