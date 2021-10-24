using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message
{
    /* Реализует команду "Коллекция" */
    public class Collection : MessageCommand
    {
        protected override string CommandText => Text.collection;
        public override async Task Execute()
        {
            /* Очищаем чат с пользователем */
            await User.ClearChat();
            /* Переводим состояние пользователя в меню коллекции */
            User.Session.State = UserState.CollectionMenu;
            User.Session.InitNewModule<CollectionModule>();
            /* Отображаем сообщение с фильтрами */
            await new ShowFiltersMenu(User, Update).Execute();
        }
        
        public Collection() { }
        public Collection(UserEntity user, Update update) : base(user, update) { }
    }
}