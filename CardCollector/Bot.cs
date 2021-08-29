using Telegram.Bot;

namespace CardCollector
{
    using static Resources.AppSettings;
    public static class Bot
    {
        private static TelegramBotClient _client;
        public static TelegramBotClient Client => _client ??= new TelegramBotClient(TOKEN);
    }
}