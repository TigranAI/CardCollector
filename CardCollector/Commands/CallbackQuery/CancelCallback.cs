using System.Threading.Tasks;
using CardCollector.Commands.Message.TextMessage;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class CancelCallback : CallbackQuery
    {
        protected override string CommandText => Command.cancel;
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
            User.Session.State = UserState.Default;
            EnterEmojiMessage.RemoveFromQueue(User.Id);
            EnterGemsPriceMessage.RemoveFromQueue(User.Id);
            await User.ClearChat();
        }

        public CancelCallback() { }
        public CancelCallback(UserEntity user, Update update) : base(user, update) { }
    }
}