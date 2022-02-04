using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using Telegram.Bot.Types;

namespace CardCollector.Commands
{
    [Attributes.CallbackQuery, Attributes.Abstract]
    public abstract class CallbackQueryHandler : HandlerModel
    {
        public static readonly List<Type> Commands = new();

        protected string CallbackData;
        protected string CallbackQueryId;

        public static async Task<HandlerModel> Factory(Update update)
        {
            var user = await UserDao.GetUser(update.CallbackQuery!.From);
            if (user.IsBlocked) return new IgnoreHandler();

            foreach (var handlerType in Commands)
            {
                var handler = (HandlerModel) Activator.CreateInstance(handlerType, user, update);
                if (handler.Match(user, update)) return handler;
            }
            
            return new IgnoreHandler();
        }

        protected internal override bool Match(UserEntity user, Update update)
        {
            var query = update.CallbackQuery!.Data!.Split('=')[0];
            return query == CommandText;
        }

        protected CallbackQueryHandler(UserEntity user, Update update) : base(user, update)
        {
            CallbackData = update.CallbackQuery!.Data;
            CallbackQueryId = update.CallbackQuery!.Id;
        }
    }
}