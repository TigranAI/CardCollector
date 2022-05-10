using System;
using System.Collections.Generic;
using System.Linq;
using CardCollector.Commands.ChosenInlineResultHandler;
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
        
        public static IEnumerable<InlineQueryResult> ToTelegramStickers(
            this IEnumerable<Sticker> list,
            string command,
            int offset)
        {
            return list
                .Skip(offset)
                .Take(50)
                .Select(sticker => sticker.AsTelegramCachedSticker(command));
        }
        
        public static IEnumerable<InlineQueryResult> ToTelegramStickers(
            this IEnumerable<UserSticker> list,
            string command,
            int offset)
        {
            return list
                .Skip(offset)
                .Take(50)
                .Select(sticker => sticker.AsTelegramCachedSticker(command));
        }

        public static IEnumerable<InlineQueryResult> ToTelegramStickersAsMessage(this IEnumerable<Sticker> list,
            string command, int offset)
        {
            return list
                .Skip(offset)
                .Take(50)
                .Select(sticker =>
                {
                    var result = sticker.AsTelegramCachedSticker(command);
                    result.InputMessageContent = new InputTextMessageContent(Text.select);
                    return result;
                });
        }

        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
        {
            return source.Select((item, index) => (item, index));
        }
        
        public static InlineQueryResultArticle AsTelegramBetArticle(this UserSticker sticker)
        {
            var betMessage = new InputTextMessageContent(
                $"{sticker.User.Username} {Text.bet} {sticker.Sticker.Title} {sticker.Sticker.TierAsStars()}");
            return new InlineQueryResultArticle
                ($"{ChosenInlineResultCommands.made_a_bet}={sticker.Id}", sticker.Sticker.Title, betMessage)
                {
                    Description = $"{Text.tier}: {sticker.Sticker.TierAsStars()} | {Text.count}: {sticker.Count}",
                    ReplyMarkup = Keyboard.AnswerBet
                };
        }
    }
}