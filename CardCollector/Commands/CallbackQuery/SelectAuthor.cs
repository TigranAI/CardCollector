using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    /* Реализует нажатие на кнопку "Автор" (открывается меню с выбором автора) */
    public class SelectAuthor : CallbackQueryCommand
    {
        protected override string CommandText => Command.author;

        public override async Task Execute()
        {
            var page = int.Parse(CallbackData.Split('=')[1]);
            /* Получаем из бд список всех авторов */
            var list = await StickerDao.GetAuthorsList();
            /* Сортируем по алфавиту */
            list.Sort();
            /* Заменяем сообщение меню на сообщение со списком */
            await MessageController.EditMessage(User, CallbackMessageId,
                Messages.choose_author, Keyboard.GetAuthorsKeyboard(list, page));
        }

        public SelectAuthor() { }
        public SelectAuthor(UserEntity user, Update update) : base(user, update) { }
    }
}