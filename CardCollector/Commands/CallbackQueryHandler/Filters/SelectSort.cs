using System.Threading.Tasks;
using CardCollector.Resources;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.CallbackQueryHandler.Filters
{
    public class SelectSort : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.sort;

        protected override async Task Execute()
        {
            await User.Messages.EditMessage(Messages.choose_sort, Keyboard.SortOptions);
        }
    }
}