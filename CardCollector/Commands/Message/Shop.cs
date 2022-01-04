using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.Message
{
    /* Реализует команду "Магазин" */
    public class Shop : MessageCommand
    {
        protected override string CommandText => Text.shop;
        protected override bool ClearMenu => true;
        protected override bool AddToStack => true;
        protected override bool ClearStickers => true;

        public override async Task Execute()
        {
            User.Session.InitNewModule<ShopModule>();
            var haveSpecialOffers = await (await ShopDao.GetSpecialPositions())
                .AnyAsync(async offer => offer.IsInfinite || !await SpecialOfferUsersDao.NowUsed(User.Id, offer.Id));
            await MessageController.EditMessage(User, Messages.shop_message, Keyboard.ShopKeyboard(haveSpecialOffers, User.PrivilegeLevel),
                ParseMode.Html);
        }
        
        public Shop() { }
        public Shop(UserEntity user, Update update) : base(user, update) 
        {
            /* Переводим состояние пользователя в меню магазина */
            User.Session.State = UserState.ShopMenu;
            
        }
    }
}