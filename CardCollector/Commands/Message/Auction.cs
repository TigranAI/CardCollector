using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message
{
    /* Реалищует команду "Аукцион" */
    public class Auction : MessageCommand
    {
        protected override string CommandText => Text.auction;
        protected override bool ClearMenu => true;
        protected override bool AddToStack => true;
        protected override bool ClearStickers => true;

        public override async Task Execute()
        {
            /* Отображаем сообщение с фильтрами */
            await new ShowFiltersMenu(User, Update).Execute();
        }
        
        public Auction() { }

        public Auction(UserEntity user, Update update) : base(user, update)
        {
            /* Переводим состояние пользователя в меню аукциона */
            User.Session.State = UserState.AuctionMenu;
        }
    }
}