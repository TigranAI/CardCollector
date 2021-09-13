﻿using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands
{
    public class IgnoreUpdate : UpdateModel
    {
        protected override string Command => "";
        public override async Task<Telegram.Bot.Types.Message> Execute()
        {
            if (Update.Message?.Chat.Type is ChatType.Private)
                await MessageController.DeleteMessage(User, Update.Message.MessageId);
            return new Telegram.Bot.Types.Message();
        }
        
        public IgnoreUpdate(UserEntity user, Update update) : base(user, update) { }
        public IgnoreUpdate() { }
    }
}