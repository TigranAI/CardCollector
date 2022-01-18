// ReSharper disable ConditionIsAlwaysTrueOrFalse

using System.Globalization;

namespace CardCollector.Resources
{
    public static class Constants
    {
        /* Переключить данный флаг при сборке на сервер */
        public static bool DEBUG = false;

        /* Интервал сохранения изменений */
        public static readonly double SAVING_CHANGES_INTERVAL = DEBUG ? 10 * 1000 : 5 * 60 * 1000;
        /* Время кэширования результатов @имя_бота команд */
        public const int INLINE_RESULTS_CACHE_TIME = 1;
        /* Включает бесконечные стикеры без наличия их в коллекции */
        public static readonly bool UNLIMITED_ALL_STICKERS = false;
        /* Время простоя удаления пользователей */
        public static readonly int SESSION_ACTIVE_PERIOD = DEBUG ? 1 : 60;

        /* Количество стикеров для создания комбинации */
        public const int COMBINE_COUNT = 5;

        public const int TEST_ALERTS_INTERVAL = 1;

        public static DateTimeFormatInfo TimeCulture = new CultureInfo("ru-RU", false).DateTimeFormat;
    }
}