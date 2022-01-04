// ReSharper disable ConditionIsAlwaysTrueOrFalse

namespace CardCollector.Resources
{
    public static class Constants
    {
        /* Переключить данный флаг при сборке на сервер */
        public const bool DEBUG = true;

        /* Интервал сохранения изменений */
        public const double SAVING_CHANGES_INTERVAL = DEBUG ? 10 * 1000 : 5 * 60 * 1000;
        /* Время кэширования результатов @имя_бота команд */
        public const int INLINE_RESULTS_CACHE_TIME = 1;
        /* Включает бесконечные стикеры без наличия их в коллекции */
        public static readonly bool UNLIMITED_ALL_STICKERS = false;
        /* Время простоя удаления пользователей */
        public const int SESSION_ACTIVE_PERIOD = DEBUG ? 1 : 60;

        /* Количество стикеров для создания комбинации */
        public const int COMBINE_COUNT = 5;

        public const int TEST_ALERTS_INTERVAL = 1;
    }
}