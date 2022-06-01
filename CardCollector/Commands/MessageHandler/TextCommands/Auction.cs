using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace CardCollector.Commands.MessageHandler.TextCommands;

public class Auction : MessageHandler
{
    protected override string CommandText => MessageCommands.auction_text;
    protected override async Task Execute()
    {
        await new Commands.MessageHandler.Auction.Auction()
            .Init(User, Context, new Update() {Message = Message})
            .PrepareAndExecute();
    }
}