﻿using System.Threading.Tasks;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.CallbackQueryHandler.Shop
{
    public class SelectOffer : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.select_order;

        protected override async Task Execute()
        {
            var orderId = int.Parse(CallbackQuery.Data!.Split('=')[1]);
            var orderInfo = await Context.SpecialOrders.FindById(orderId);
            if (orderInfo == null) return;
            User.Session.GetModule<ShopModule>().SelectedOrderId = orderId;
            if (orderInfo.PreviewFileId == null)
                await User.Messages.EditMessage(User, orderInfo.Title, Keyboard.OrderKeyboard(orderInfo));
            else
                await User.Messages.SendSticker(User, orderInfo.PreviewFileId, Keyboard.OrderKeyboard(orderInfo));
        }
    }
}