using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler
{
    [Attributes.CallbackQueryHandler]
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
                if (Attribute.IsDefined(type, typeof(Attributes.CallbackQueryHandler)))
                    Commands.Add(type);
            }
        }

        public static async Task<HandlerModel> Factory(Update update)
        {
            var context = new BotDatabaseContext();
            var user = await context.Users.FindUserWithSession(update.CallbackQuery!.From);
            if (user.IsBlocked) return new IgnoreHandler(user, context);

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

        protected CallbackQueryHandler(User user, CallbackQuery callbackQuery, BotDatabaseContext context) : base(user, context)
        {
            CallbackQuery = callbackQuery;
        }
    }
}