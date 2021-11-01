// ReSharper disable ConditionIsAlwaysTrueOrFalse

namespace CardCollector.Resources
{
    public static class Constants
    {
        /* Переключить данный флаг при сборке на сервер */
        public const bool DEBUG = false;

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


        public const int AuctionDao = 1;
        public const int CashDao = 2;
        public const int DailyTaskDao = 3;
        public const int LevelDao = 4;
        public const int PacksDao = 5;
        public const int SessionTokenDao = 6;
        public const int SettingsDao = 7;
        public const int ShopDao = 8;
        public const int SpecialOfferUsersDao = 9;
        public const int StickerDao = 10;
        public const int UserDao = 11;
        public const int UserLevelDao = 12;
        public const int UserPacksDao = 13;
        public const int UserStickerRelationDao = 14;
    }
}