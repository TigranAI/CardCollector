using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.ChosenInlineResultHandler
{
    [Attributes.Handlers.ChosenInlineResultHandler]
    public abstract class ChosenInlineResultHandler : HandlerModel
    {
        protected ChosenInlineResult ChosenInlineResult;

        public static readonly ICollection<Type> Commands;
        
        static ChosenInlineResultHandler()
        {
            Commands = new LinkedList<Type>();
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes())
            {
                if (type == typeof(ChosenInlineResultHandler)) continue;
                if (Attribute.IsDefined(type, typeof(Attributes.Handlers.ChosenInlineResultHandler)))
                    Commands.Add(type);
            }
        }

        public static async Task<HandlerModel> Factory(Update update)
        {
            var context = new BotDatabaseContext();
            var user = await context.Users.FindUser(update.ChosenInlineResult!.From);
            if (user.IsBlocked) return new IgnoreHandler();

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