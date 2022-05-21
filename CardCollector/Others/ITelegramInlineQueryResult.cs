using Telegram.Bot.Types.InlineQueryResults;

namespace CardCollector.Others;

public interface ITelegramInlineQueryResult
{
    public InlineQueryResult ToResult(string command);
}