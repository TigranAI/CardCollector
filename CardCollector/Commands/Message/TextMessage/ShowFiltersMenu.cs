using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message.TextMessage
{
    /* Этот класс реализует отправку нового сообщения с фильтрами пользователя */
    public class ShowFiltersMenu : MessageCommand
    {
        protected override string CommandText => "Message";
        public override async Task Execute()
        {
            switch (User.Session.State)
            {
                case UserState.CollectionMenu:
                    User.Session.DeleteModule<CollectionModule>();
                    break;
                case UserState.ShopMenu:
                    User.Session.DeleteModule<ShopModule>();
                    break;
                case UserState.AuctionMenu:
                    User.Session.DeleteModule<AuctionModule>();
                    break;
                case UserState.CombineMenu:
                    User.Session.DeleteModule<CombineModule>();
                    break;
                case UserState.ProductMenu:
                    User.Session.DeleteModule<AuctionModule>();
                    break;
                case UserState.Default:
                    User.Session.GetModule<DefaultModule>().Reset();
                    break;
            }
            /* Формируем сообщение с имеющимися фильтрами у пользователя */
            var text = User.Session.GetModule<FiltersModule>().ToString(User.Session.State);
            /* Отправляем сообщение */
            var message = await MessageController.SendMessage(User, text, Keyboard.GetSortingMenu(User.Session.State));
            /* Добавляем это сообщение в список для удаления */
            User.Session.Messages.Add(message.MessageId);
        }
        
        public ShowFiltersMenu(UserEntity user, Update update) : base(user, update) { }
    }
}