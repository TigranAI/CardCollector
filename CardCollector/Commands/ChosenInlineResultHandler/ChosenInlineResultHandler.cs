using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.ChosenInlineResultHandler
{
    public abstract class ChosenInlineResultHandler : HandlerModel
    {
        protected ChosenInlineResult ChosenInlineResult;

        public static readonly ICollection<Type> Commands = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type => type.IsSubclassOf(typeof(ChosenInlineResultHandler)) && !type.IsAbstract)
            .ToList();

        public static async Task<HandlerModel> Factory(Update update)
        {
            var context = new BotDatabaseContext();
            var user = await context.Users.FindUser(update.ChosenInlineResult!.From);
            if (user.IsBlocked) return new IgnoreHandler();
            await context.SaveChangesAsync();
            
            user.InitSession();
            
            foreach (var handlerType in Commands)
            {
                var handler = (HandlerModel?) Activator.CreateInstance(handlerType);
                if (handler != null && handler.Init(user, context, update).Match()) return handler;
            }
            
            return new IgnoreHandler();
        }

        public override bool Match()
        {
            var query = ChosenInlineResult.ResultId.Split("=")[0];
            return CommandText == query;
        }

        public override HandlerModel Init(User user, BotDatabaseContext context, Update update)
        {
            ChosenInlineResult = update.ChosenInlineResult!;
            return base.Init(user, context, update);
        }
    }
}