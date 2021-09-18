using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Custom
{
    /* Этот класс реализует отправку нового сообщения с фильтрами пользователя */
    public class ShowFiltersMenu : Message.Message
    {
        protected override string CommandText => "Message";
        public override async Task Execute()
        {
            /* Формируем сообщение с имеющимися фильтрами у пользователя */
            var text = $"{Messages.current_filters}\n" +
                       $"{Messages.author} {(User.Filters["author"].Equals("") ? Messages.all : User.Filters["author"])}\n" +
                       $"{Messages.tier} {(User.Filters["tier"].Equals(-1) ? Messages.all : new string('⭐', (int)User.Filters["tier"]))}\n" +
                       $"{Messages.emoji} {(User.Filters["emoji"].Equals("") ? Messages.all : User.Filters["emoji"])}\n" +
                       $"{Messages.price} {User.Filters["price"]}-∞\n" +
                       $"{Messages.sorting} {User.Filters["sorting"]}\n" +
                       $"\n{Messages.select_filter}";
            /* Отправляем сообщение */
            var message = await MessageController.SendMessage(User, text, Keyboard.SortingOptions);
            /* Добавляем это сообщение в список для удаления */
            User.Messages.Add(message.MessageId);
        }
        
        public ShowFiltersMenu(UserEntity user, Update update) : base(user, update) { }
    }
}