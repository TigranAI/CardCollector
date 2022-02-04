using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    /* Реализует нажатие на кнопку "Автор" (открывается меню с выбором автора) */
    public class AuthorsMenu : CallbackQueryHandler
    {
        protected override string CommandText => Command.authors_menu;
        protected override bool AddToStack => true;

        public override async Task Execute()
        {
            var offset = int.Parse(CallbackData.Split('=')[1]);
            /* Получаем из бд список всех авторов */
            var packs = await PacksDao.GetNext(offset, 10);
            if (packs.Count == 0)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.page_not_found);
            /* Заменяем сообщение меню на сообщение со списком */
            else
                await MessageController.EditMessage(User, Messages.choose_author,
                    Keyboard.GetAuthorsKeyboard(packs, offset, await PacksDao.GetCount()));
        }

        public AuthorsMenu(UserEntity user, Update update) : base(user, update) { }
    }
}