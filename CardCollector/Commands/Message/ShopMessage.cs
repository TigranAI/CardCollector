using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message
{
    /* Реализует команду "Магазин" */
    public class ShopMessage : Message
    {
        protected override string CommandText => Text.shop;
        public override async Task Execute()
        {
            /* Очищаем чат с пользователем */
            await User.ClearChat();
            /* Переводим состояние пользователя в меню магазина */
            User.State = UserState.ShopMenu;
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
        
        public ShopMessage() { }
        public ShopMessage(UserEntity user, Update update) : base(user, update) { }
    }
}