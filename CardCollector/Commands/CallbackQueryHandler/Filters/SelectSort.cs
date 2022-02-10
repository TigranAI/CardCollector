using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.Resources;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

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