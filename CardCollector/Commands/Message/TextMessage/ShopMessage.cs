using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message.TextMessage
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
            User.Session.State = UserState.ShopMenu;
            /* Отображаем сообщение с фильтрами */
            await new ShowFiltersMenu(User, Update).Execute();
        }
        
        public ShopMessage() { }
        public ShopMessage(UserEntity user, Update update) : base(user, update) { }
    }
}