namespace CardCollector.Resources
{
    public static class Constants
    {
        public const bool DEBUG = true;

        public const int MEMORY_CLEANER_INTERVAL = 60 * 1000;
        public const double SAVING_CHANGES_INTERVAL = DEBUG ? 10 * 1000 : 5 * 60 * 1000;
        public const int INLINE_RESULTS_CACHE_TIME = 5;
    }
}