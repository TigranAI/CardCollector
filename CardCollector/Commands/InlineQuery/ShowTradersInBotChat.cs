﻿using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.InlineQuery
{
    public class ShowTradersInBotChat : InlineQueryHandler
    {
        public override async Task Execute()
        {
            var module = User.Session.GetModule<AuctionModule>();
            // Получаем список продавцов
            var traders = await AuctionController.GetTradersList(Query, module.SelectedSticker.Id);
            var results = User.Session.GetModule<FiltersModule>()
                .ApplyPriceTo(traders)
                .ToTelegramResults(Command.buy_sticker, 1.0 - await User.AuctionDiscount()/100.0);
            // Посылаем пользователю ответ на его запрос
            await MessageController.AnswerInlineQuery(InlineQueryId, await results);
        }
        
        protected internal override bool Match(UserEntity user, Update update)
        {
            return update.InlineQuery?.ChatType is ChatType.Sender && user.Session.State == UserState.ProductMenu;
        }

        public ShowTradersInBotChat(UserEntity user, Update update) : base(user, update) { }
    }
}