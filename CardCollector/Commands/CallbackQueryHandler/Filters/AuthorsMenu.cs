using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Filters
{
    public class AuthorsMenu : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.authors_menu;
        protected override bool AddToStack => true;

        protected override async Task Execute()
        {
            var offset = int.Parse(CallbackQuery.Data!.Split('=')[1]);
            var packs = await Context.Packs.FindNext(offset, 10);
            var packsCount = Context.Packs.Count() - 1;
            if (packs.Count == 0)
                await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.page_not_found);
            else
                await User.Messages.EditMessage(User, Messages.choose_author,
                    Keyboard.GetAuthorsKeyboard(packs, offset, packsCount));
        }

        public AuthorsMenu(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery)
        {
        }
    }
}