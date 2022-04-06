using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes.Menu;
using CardCollector.Controllers;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.CallbackQueryHandler.Filters
{
    [MenuPoint]
    public class AuthorsMenu : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.authors_menu;

        protected override async Task Execute()
        {
            var offset = int.Parse(CallbackQuery.Data!.Split('=')[1]);
            var packs = await Context.Packs.FindNextSkipRandom(offset, 10);
            var packsCount = Context.Packs.Count() - 1;
            if (packs.Count == 0)
                await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.page_not_found);
            else
                await User.Messages.EditMessage(User, Messages.choose_author,
                    Keyboard.GetAuthorsKeyboard(packs, offset, packsCount));
        }
    }
}