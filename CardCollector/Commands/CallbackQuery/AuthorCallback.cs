using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    /* Реализует нажатие на кнопку "Автор" (открывается меню с выбором автора) */
    public class AuthorCallback : CallbackQuery
    {
        protected override string Command => CallbackQueryCommands.author;

        public override async Task Execute()
        {
            /* Получаем из бд список всех авторов */
            var list = await StickerDao.GetAuthorsList();
            /* Сортируем по алфавиту */
            list.Sort();
            /* Заменяем сообщение меню на сообщение со списком */
            await MessageController.EditMessage(User, CallbackMessageId,
                Messages.choose_author, Keyboard.GetAuthorsKeyboard(list));
        }

        public AuthorCallback() { }
        public AuthorCallback(UserEntity user, Update update) : base(user, update) { }
    }
}