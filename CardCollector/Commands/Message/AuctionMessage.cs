using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message
{
    /* Реалищует команду "Аукцион" */
    public class AuctionMessage : Message
    {
        protected override string Command => MessageCommands.shop;
        public override async Task Execute()
        {
            /* Очищаем чат с пользователем */
            await User.ClearChat();
            /* Переводим состояние пользователя в меню аукциона */
            User.State = UserState.AuctionMenu;
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
        
        public AuctionMessage() { }
        public AuctionMessage(UserEntity user, Update update) : base(user, update) { }
    }
}