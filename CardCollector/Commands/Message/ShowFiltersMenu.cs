using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message
{
    /* Этот класс реализует отправку нового сообщения с фильтрами пользователя */
    public class ShowFiltersMenu : MessageCommand
    {
        protected override string CommandText => "";

        public override async Task Execute()
        {
            /* Формируем сообщение с имеющимися фильтрами у пользователя */
            var text = User.Session.GetModule<FiltersModule>().ToString(User.Session.State);
            /* Отправляем сообщение */
            await MessageController.SendMessage(User, text, Keyboard.GetSortingMenu(User.Session.State));
        }
        
        public ShowFiltersMenu(UserEntity user, Update update) : base(user, update) { }
    }
}