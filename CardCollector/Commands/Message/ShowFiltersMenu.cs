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
        protected override bool ClearMenu => false;
        protected override bool AddToStack => false;

        public override async Task Execute()
        {
            /* Формируем сообщение с имеющимися фильтрами у пользователя */
            var text = User.Session.GetModule<FiltersModule>().ToString(User.Session.State);
            /* Отправляем сообщение */
            var message = await MessageController.SendMessage(User, text, Keyboard.GetSortingMenu(User.Session.State));
            /* Добавляем это сообщение в список для удаления */
            User.Session.Messages.Add(message.MessageId);
        }
        
        public ShowFiltersMenu(UserEntity user, Update update) : base(user, update) { }
    }
}