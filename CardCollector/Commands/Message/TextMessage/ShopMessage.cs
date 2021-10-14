﻿using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
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
            User.Session.InitNewModule<ShopModule>();
            var specialOffers = await ShopDao.GetSpecialPositions();
            var message = await MessageController.SendMessage(User, Messages.shop_message, 
                Keyboard.ShopKeyboard(specialOffers));
            User.Session.Messages.Add(message.MessageId);
        }
        
        public ShopMessage() { }
        public ShopMessage(UserEntity user, Update update) : base(user, update) { }
    }
}