using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Filters
{
    public class SelectSort : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.sort;

        protected override async Task Execute()
        {
            await User.Messages.EditMessage(User, Messages.choose_sort, Keyboard.SortOptions);
        }

        public SelectSort(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery)
        {
        }
    }
}