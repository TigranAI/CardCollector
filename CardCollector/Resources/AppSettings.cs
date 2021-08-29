namespace CardCollector.Resources
{
    using static Constants;
    public static class AppSettings
    {
        public const string TOKEN = DEBUG ? "1947951335:AAFX_kMdRbM_lih3dGUdvQ4d4-0qQzy7igg" : "";
        public const string NAME = DEBUG ? "TigranCardCollectorBot" : "";
        public const string DB_IP = "localhost";
        public const string DB_PORT = "3306";
        public const string DB_UID = "card_collector";
        public const string DB_PWD = DEBUG ? "Password1*" : "";
        public const string DB_SCHEMA = DEBUG ? "card_collector" : "";
    }
}