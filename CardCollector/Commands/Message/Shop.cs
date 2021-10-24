using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message
{
    /* Реализует команду "Магазин" */
    public class Shop : MessageCommand
    {
        protected override string CommandText => Text.shop;
        protected override bool ClearMenu => true;
        protected override bool AddToStack => true;

        public override async Task Execute()
        {
            /* Очищаем чат с пользователем */
            await User.ClearChat();
            /* Переводим состояние пользователя в меню магазина */
            User.Session.State = UserState.ShopMenu;
            User.Session.InitNewModule<ShopModule>();
            var haveSpecialOffers = await (await ShopDao.GetSpecialPositions())
                .AnyAsync(async offer => offer.IsInfinite || !await SpecialOfferUsersDao.NowUsed(User.Id, offer.Id));
            var message = await MessageController.SendMessage(User, Messages.shop_message, Keyboard.ShopKeyboard(haveSpecialOffers));
            User.Session.Messages.Add(message.MessageId);
        }
        
        public Shop() { }
        public Shop(UserEntity user, Update update) : base(user, update) { }
    }
}