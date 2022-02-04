using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Commands.PreCheckoutQuery;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using Telegram.Bot.Types;

namespace CardCollector.Commands
{
    [Attributes.PreCheckoutQuery, Attributes.Abstract]
    public abstract class PreCheckoutQueryHandler : HandlerModel
    {
        protected readonly string PreCheckoutQueryId;
        protected readonly int Amount;

        public static readonly List<Type> Commands = new();
        public static async Task<HandlerModel> Factory(Update update)
        {
            var user = await UserDao.GetUser(update.PreCheckoutQuery!.From);
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
            return CommandText == update.PreCheckoutQuery!.InvoicePayload;
        }

        protected PreCheckoutQueryHandler(UserEntity user, Update update) : base(user, update)
        {
            PreCheckoutQueryId = update.PreCheckoutQuery!.Id;
            Amount = update.PreCheckoutQuery.TotalAmount;
        }
    }
}