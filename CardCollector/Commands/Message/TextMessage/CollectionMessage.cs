using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message.TextMessage
{
    /* Реализует команду "Коллекция" */
    public class CollectionMessage : Message
    {
        protected override string CommandText => Text.collection;
        public override async Task Execute()
        {
            /* Очищаем чат с пользователем */
            await User.ClearChat();
            /* Переводим состояние пользователя в меню коллекции */
            User.Session.State = UserState.CollectionMenu;
            /* Отображаем сообщение с фильтрами */
            await new ShowFiltersMenu(User, Update).Execute();
        }
        
        public CollectionMessage() { }
        public CollectionMessage(UserEntity user, Update update) : base(user, update) { }
    }
}