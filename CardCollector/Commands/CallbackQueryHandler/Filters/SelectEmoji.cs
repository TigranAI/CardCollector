using System.Threading.Tasks;
using CardCollector.Commands.MessageHandler.Menu;
using CardCollector.Database;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Filters
{
    public class SelectEmoji : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.emoji;

        protected override async Task Execute()
        {
            EnterEmoji.AddToQueue(User.Id);
            await User.Messages.EditMessage(User, Messages.enter_emoji, Keyboard.EmojiOptions);
        }

        public SelectEmoji(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery) { }
    }
}