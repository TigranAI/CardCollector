using System.Globalization;

namespace CardCollector.Resources
{
    public static class Constants
    {
        public static bool DEBUG = false;

        /* Интервал сохранения изменений */
        public static readonly double SAVING_CHANGES_INTERVAL = DEBUG ? 10 * 1000 : 5 * 60 * 1000;
        /* Время кэширования результатов @имя_бота команд */
        public const int INLINE_RESULTS_CACHE_TIME = 1;

        /* Количество стикеров для создания комбинации */
        public const int COMBINE_COUNT = 5;

        public const int TEST_ALERTS_INTERVAL = 1;

        public static DateTimeFormatInfo TimeCulture = new CultureInfo("ru-RU", false).DateTimeFormat;
    }
}