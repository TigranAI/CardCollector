using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace CardCollector.Commands.MessageHandler.TextCommands;

public class Profile : MessageHandler
{
    protected override string CommandText => MessageCommands.profile_text;
    protected override async Task Execute()
    {
        await new Commands.MessageHandler.Profile.Profile()
            .Init(User, Context, new Update() {Message = Message})
            .PrepareAndExecute();
    }
}