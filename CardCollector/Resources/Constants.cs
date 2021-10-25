using System;
// ReSharper disable ConditionIsAlwaysTrueOrFalse
#pragma warning disable 162

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
        
        /* Уровни привилегий пользователей системы */
        public const int OWNER_PRIVILEGE_LEVEL = 10;
        public const int ADMIN_PRIVILEGE_LEVEL = 9;
        public const int PROGRAMMER_PRIVILEGE_LEVEL = 7;
        public const int ARTIST_PRIVILEGE_LEVEL = 4;
        
        /* Количество стикеров для создания комбинации */
        public const int COMBINE_COUNT = 5;
        
        /* Время оповещения и сброса ежедневных заданий */
        public static readonly TimeSpan DailyTaskAlert = DEBUG ? new TimeSpan(20, 30, 0) : new TimeSpan(10, 0, 0);
        public static readonly TimeSpan DailyTaskReset = DEBUG ? new TimeSpan(20, 30, 0) : new TimeSpan(10, 0, 0);
        /* Время выдачи наград за пассивные эффекты стикеров */
        public static readonly TimeSpan DailyStickerRewardCheck = DEBUG ? new TimeSpan(20, 35, 0) : new TimeSpan(11, 0, 0);
    }
}