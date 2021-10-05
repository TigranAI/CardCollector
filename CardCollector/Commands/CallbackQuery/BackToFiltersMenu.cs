using System.Threading.Tasks;
using CardCollector.Commands.Message.TextMessage;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class BackToFiltersMenu : CallbackQuery
    {
        protected override string CommandText => Command.back;
        public override async Task Execute()
        {
            /* Удаляем пользователя из очереди */
            EnterEmojiMessage.RemoveFromQueue(User.Id);
            switch (User.Session.State)
            {
                case UserState.CombineMenu:
                    User.Session.State = UserState.CollectionMenu;
                    User.Session.DeleteModule<CombineModule>();
                    break;
                case UserState.CollectionMenu:
                    User.Session.GetModule<CollectionModule>().Reset();
                    break;
                case UserState.ProductMenu:
                    User.Session.State = UserState.AuctionMenu;
                    User.Session.GetModule<AuctionModule>().Reset();
                    break;
                case UserState.ShopMenu:
                    User.Session.GetModule<ShopModule>().Reset();
                    break;
                case UserState.AuctionMenu:
                    User.Session.GetModule<AuctionModule>().Reset();
                    break;
            }
            await User.ClearChat();
            /* Формируем сообщение с имеющимися фильтрами у пользователя */
            var filtersModule = User.Session.GetModule<FiltersModule>();
            var text = filtersModule.ToString(User.Session.State);
            /* Отправляем сообщение */
            var message = await MessageController.SendMessage(User, text, Keyboard.GetSortingMenu(User.Session.State));
            User.Session.Messages.Add(message.MessageId);
        }
        
        public BackToFiltersMenu() { }
        public BackToFiltersMenu(UserEntity user, Update update) : base(user, update) { }
    }
}