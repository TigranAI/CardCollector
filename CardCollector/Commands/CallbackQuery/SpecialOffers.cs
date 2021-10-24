﻿using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class SpecialOffers : CallbackQueryCommand
    {
        protected override string CommandText => Command.special_offers;
        protected override bool ClearMenu => false;
        protected override bool AddToStack => true;

        public override async Task Execute()
        {
            var specialOffers = await (await ShopDao.GetSpecialPositions())
                .WhereAsync(async offer => offer.IsInfinite || !await SpecialOfferUsersDao.NowUsed(User.Id, offer.Id));
            var shopEntities = specialOffers.ToList();
            if (shopEntities.Count < 1)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.offers_not_found, true);
            else
                await MessageController.EditMessage(User, CallbackMessageId, Messages.available_offers,
                    Keyboard.SpecialOffersKeyboard(shopEntities));
        }

        public SpecialOffers() { }
        public SpecialOffers(UserEntity user, Update update) : base(user, update) { }
    }
}