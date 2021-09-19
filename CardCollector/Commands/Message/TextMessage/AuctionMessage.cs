using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message.TextMessage
{
    /* Реалищует команду "Аукцион" */
    public class AuctionMessage : Message
    {
        protected override string CommandText => Text.auction;
        public override async Task Execute()
        {
            /* Очищаем чат с пользователем */
            await User.ClearChat();
            /* Переводим состояние пользователя в меню аукциона */
            User.State = UserState.AuctionMenu;
            /* Отображаем сообщение с фильтрами */
            await new ShowFiltersMenu(User, Update).Execute();
        }
        
        public AuctionMessage() { }
        public AuctionMessage(UserEntity user, Update update) : base(user, update) { }
    }
}