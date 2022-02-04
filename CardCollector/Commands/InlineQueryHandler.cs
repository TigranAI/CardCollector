using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using Telegram.Bot.Types;

namespace CardCollector.Commands
{
    [Attributes.InlineQuery, Attributes.Abstract]
    public abstract class InlineQueryHandler : HandlerModel
    {
        protected readonly string InlineQueryId;
        protected readonly string Query;

        protected override string CommandText => "";
        
        public static readonly List<Type> Commands = new();

        public override async Task PrepareAndExecute()
        {
            await Execute();
        }

        public static async Task<HandlerModel> Factory(Update update)
        {
            var user = await UserDao.GetUser(update.InlineQuery!.From);
            if (user.IsBlocked) return new IgnoreHandler();
            
            foreach (var handlerType in Commands)
            {
                var handler = (HandlerModel) Activator.CreateInstance(handlerType, user, update);
                if (handler.Match(user, update)) return handler;
            }
            
            return new IgnoreHandler();
        }

        protected InlineQueryHandler(UserEntity user, Update update) : base(user, update)
        {
            InlineQueryId = update.InlineQuery!.Id;
            Query = update.InlineQuery!.Query;
        }
    }
}