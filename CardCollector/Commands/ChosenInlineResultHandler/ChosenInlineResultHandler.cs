using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.ChosenInlineResultHandler
{
    [Attributes.ChosenInlineResultHandler]
    public abstract class ChosenInlineResultHandler : HandlerModel
    {
        protected readonly ChosenInlineResult ChosenInlineResult;

        public static readonly ICollection<Type> Commands;
        
        static ChosenInlineResultHandler()
        {
            Commands = new LinkedList<Type>();
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes())
            {
                if (type == typeof(ChosenInlineResultHandler)) continue;
                if (Attribute.IsDefined(type, typeof(Attributes.ChosenInlineResultHandler)))
                    Commands.Add(type);
            }
        }

        public static async Task<HandlerModel> Factory(Update update)
        {
            var context = new BotDatabaseContext();
            var user = await context.Users.FindUserWithSession(update.ChosenInlineResult!.From);
            if (user.IsBlocked) return new IgnoreHandler(user, context);

            foreach (var handlerType in Commands)
            {
                var handler = (HandlerModel?) Activator.CreateInstance(handlerType, user, context, update.ChosenInlineResult);
                if (handler != null && handler.Match()) return handler;
            }
            
            return new IgnoreHandler(user, context);
        }

        public override bool Match()
        {
            var query = ChosenInlineResult.ResultId.Split("=")[0];
            return CommandText == query;
        }

        protected ChosenInlineResultHandler(User user, 
            BotDatabaseContext context, ChosenInlineResult chosenInlineResult) : base(user, context)
        {
            ChosenInlineResult = chosenInlineResult;
        }
    }
}