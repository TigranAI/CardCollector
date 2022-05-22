using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Others;

public interface ITelegramInlineQueryMessageResult
{
    public InlineQueryResult ToMessageResult(string command);
}