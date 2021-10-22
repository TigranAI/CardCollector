using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class AuthorMenu : CallbackQueryCommand
    {
        protected override string CommandText => Command.author_menu;
        public override async Task Execute()
        {
            var packs = await UsersPacksDao.GetUserPacks(User.Id);
            if (packs.AuthorCount == 0)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.packs_count_zero, true);
            else
            {
                var page = int.Parse(CallbackData.Split('=')[1]);
                var low = page * 10 - 10;
                var up = page * 10;
                var authors = await PacksDao.GetAll();
                authors = authors.Where((_, index) => index >= low && index < up).ToList();
                if (authors.Count == 0)
                    await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.page_not_found);
                else
                    await MessageController.EditMessage(User, CallbackMessageId, Messages.choose_author,
                        Keyboard.GetAuthorsKeyboard(authors, page));
            }
        }

        public AuthorMenu() { }
        public AuthorMenu(UserEntity user, Update update) : base(user, update) { }
    }
}