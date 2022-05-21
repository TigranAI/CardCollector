using System.Threading.Tasks;
using CardCollector.Commands.MessageHandler.Menu;
using CardCollector.Resources;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.CallbackQueryHandler.Filters
{
    public class SelectEmoji : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.emoji;

        protected override async Task Execute()
        {
            EnterEmoji.AddToQueue(User.Id);
            await User.Messages.EditMessage(Messages.enter_emoji, Keyboard.EmojiOptions);
        }
    }
}