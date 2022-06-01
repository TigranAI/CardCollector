using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace CardCollector.Commands.MessageHandler.TextCommands;

public class Shop : MessageHandler
{
    protected override string CommandText => MessageCommands.shop_text;

    protected override async Task Execute()
    {
        await new Commands.MessageHandler.Shop.Shop()
            .Init(User, Context, new Update() {Message = Message})
            .PrepareAndExecute();
    }
}