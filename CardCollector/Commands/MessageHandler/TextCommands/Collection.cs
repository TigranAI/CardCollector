using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace CardCollector.Commands.MessageHandler.TextCommands;

public class Collection : MessageHandler
{
    protected override string CommandText => MessageCommands.collection_text;

    protected override async Task Execute()
    {
        await new Commands.MessageHandler.Collection.Collection()
            .Init(User, Context, new Update() {Message = Message})
            .PrepareAndExecute();
    }
}