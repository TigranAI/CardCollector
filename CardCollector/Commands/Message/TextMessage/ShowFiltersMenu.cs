using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message.TextMessage
{
    /* Этот класс реализует отправку нового сообщения с фильтрами пользователя */
    public class ShowFiltersMenu : Message
    {
        protected override string CommandText => "Message";
        public override async Task Execute()
        {
            User.Session.SelectedSticker = null;
            /* Формируем сообщение с имеющимися фильтрами у пользователя */
            var text = User.Session.Filters.ToMessage(User.Session.State);
            /* Отправляем сообщение */
            var message = await MessageController.SendMessage(User, text, Keyboard.GetSortingMenu(User.Session.State));
            /* Добавляем это сообщение в список для удаления */
            User.Session.Messages.Add(message.MessageId);
        }
        
        public ShowFiltersMenu(UserEntity user, Update update) : base(user, update) { }
    }
}