using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler
{
    [Attributes.Handlers.CallbackQueryHandler]
    public abstract class CallbackQueryHandler : HandlerModel
    {
        protected CallbackQuery CallbackQuery;
        
        public static readonly ICollection<Type> Commands;

        static CallbackQueryHandler()
        {
            Commands = new LinkedList<Type>();
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes())
            {
                if (type == typeof(CallbackQueryHandler)) continue;
                if (Attribute.IsDefined(type, typeof(Attributes.Handlers.CallbackQueryHandler)))
                    Commands.Add(type);
            }
        }

        public static async Task<HandlerModel> Factory(Update update)
        {
            var context = new BotDatabaseContext();
            var user = await context.Users.FindUser(update.CallbackQuery!.From);
            if (user.IsBlocked) return new IgnoreHandler(user, context);
            
            user.InitSession();

            foreach (var handlerType in Commands)
            {
                var handler = (HandlerModel?) Activator.CreateInstance(handlerType, user, context, update.CallbackQuery);
                if (handler != null && handler.Match()) return handler;
            }
            
            return new IgnoreHandler(user, context);
        }

        public override bool Match()
        {
            var query = CallbackQuery.Data!.Split('=')[0];
            return query == CommandText;
        }

        protected CallbackQueryHandler(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context)
        {
            CallbackQuery = callbackQuery;
        }
    }
}