using System;
using System.Collections.Generic;
using System.Globalization;
using CardCollector.Commands.MessageHandler;
using Telegram.Bot.Types;
// ReSharper disable InconsistentNaming
// ReSharper disable ConditionalTernaryEqualBranch

namespace CardCollector.Resources
{
    public static class Constants
    {
        public static bool DEBUG = false;
        
        /* Время кэширования результатов @имя_бота команд */
        public const int INLINE_RESULTS_CACHE_TIME = 1;

        /* Количество стикеров для создания комбинации */
        public const int COMBINE_COUNT = 5;

        public const int TEST_ALERTS_INTERVAL = 60;
        public static readonly int ROULETTE_MIN_PLAYERS = DEBUG ? 2 : 2;

        public static readonly long ROULETTE_INTERVAL = DEBUG ? 5 * 60 * 1000 : 5 * 60 * 1000;
        public static readonly long PUZZLE_INTERVAL = DEBUG ? 2 * 60 * 1000 : 2 * 60 * 1000;

        public static DateTimeFormatInfo TimeCulture = new CultureInfo("ru-RU", false).DateTimeFormat;
        
        public static readonly IEnumerable<BotCommand> PrivateCommands = new[]
        {
            /*new BotCommand {Command = Text.start, Description = "Запуск бота"},*/
            new BotCommand {Command = MessageCommands.menu, Description = "Показать меню"},
            new BotCommand {Command = MessageCommands.help, Description = "Показать информацию"},
            new BotCommand {Command = MessageCommands.profile_text, Description = "Профиль"},
            new BotCommand {Command = MessageCommands.shop_text, Description = "Магазин"},
            new BotCommand {Command = MessageCommands.collection_text, Description = "Коллекция"},
            new BotCommand {Command = MessageCommands.auction_text, Description = "Аукцион"},
            /*new BotCommand {Command = "/error", Description = "Сообщить об ошибке"},*/
        };
        
        public static readonly IEnumerable<BotCommand> GroupCommands = new[]
        {
            new BotCommand {Command = MessageCommands.roulette, Description = "Запустить рулетку"},
            new BotCommand {Command = MessageCommands.start_puzzle, Description = "Запустить пазл"},
            new BotCommand {Command = MessageCommands.disable_distributions, Description = "Отключить рассылки"},
            new BotCommand {Command = MessageCommands.enable_distributions, Description = "Включить рассылки"},
            /*new BotCommand {Command = "/error", Description = "Сообщить об ошибке"},*/
        };
        
        
        public static readonly TimeSpan UNBLOCK_INVITE_INTERVAL = new (14, 0, 0, 0);
        public static readonly TimeSpan BEGINNERS_TASKS_INTERVAL = new (10, 0, 0, 0);
    }
}