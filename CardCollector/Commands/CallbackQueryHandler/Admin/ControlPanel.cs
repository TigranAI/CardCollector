﻿using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.Resources;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Admin
{
    public class ControlPanel : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.control_panel;
        protected override bool AddToStack => true;

        protected override async Task Execute()
        {
            await User.Messages.EditMessage(User, Messages.control_panel, 
                Keyboard.ControlPanel(User.PrivilegeLevel), ParseMode.Html);
        }

        public override bool Match()
        {
            return base.Match() && User.PrivilegeLevel > PrivilegeLevel.Programmer;
        }

        public ControlPanel(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery) { }
    }
}