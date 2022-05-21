using Telegram.Bot.Types.InlineQueryResults;

namespace CardCollector.Others;

public interface ITelegramInlineQueryMessageResult
{
    public InlineQueryResult ToMessageResult(string command);
}