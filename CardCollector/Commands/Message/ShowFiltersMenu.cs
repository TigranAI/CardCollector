using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message
{
    /* Этот класс реализует отправку нового сообщения с фильтрами пользователя */
    public class ShowFiltersMenu : Message
    {
        protected override string CommandText => "Message";
        public override async Task Execute()
        {
            /* Формируем сообщение с имеющимися фильтрами у пользователя */
            var text = $"{Messages.current_filters}\n" +
                       $"{Messages.author} {(User.Filters[Command.author].Equals("") ? Messages.all : User.Filters[Command.author])}\n" +
                       $"{Messages.tier} {(User.Filters[Command.tier].Equals(-1) ? Messages.all : new string('⭐', (int) User.Filters[Command.tier]))}\n" +
                       $"{Messages.emoji} {(User.Filters[Command.emoji].Equals("") ? Messages.all : User.Filters[Command.emoji])}\n";
            if (User.State != UserState.CollectionMenu) text += $"{Messages.price} {User.Filters[Command.price]} -" +
                                                                $" {(User.Filters[Command.price_to] is int p && p != 0 ? p : "∞")}\n";
            text += $"{Messages.sorting} {User.Filters[Command.sort]}\n\n{Messages.select_filter}";
            /* Отправляем сообщение */
            var message = await MessageController.SendMessage(User, text, Keyboard.GetSortingMenu(User.State));
            /* Добавляем это сообщение в список для удаления */
            User.Messages.Add(message.MessageId);
        }
        
        public ShowFiltersMenu(UserEntity user, Update update) : base(user, update) { }
    }
}