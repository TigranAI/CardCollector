using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using Telegram.Bot.Types;

namespace CardCollector.Commands
{
    [Attributes.ChosenInlineResult, Attributes.Abstract]
    public abstract class ChosenInlineResultHandler : HandlerModel
    {
        protected readonly string InlineResult;
        protected readonly string InlineQuery;

        public static readonly List<Type> Commands = new();

        /* Метод, создающий объекты команд исходя из полученного обновления */
        public static async Task<HandlerModel> Factory(Update update)
        {
            var user = await UserDao.GetUser(update.ChosenInlineResult!.From);
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
            var query = update.ChosenInlineResult!.ResultId.Split("=")[0];
            return CommandText == query;
        }

        protected ChosenInlineResultHandler(UserEntity user, Update update) : base(user, update)
        {
            InlineResult = update.ChosenInlineResult!.ResultId;
            InlineQuery = update.ChosenInlineResult.Query;
        }
    }
}