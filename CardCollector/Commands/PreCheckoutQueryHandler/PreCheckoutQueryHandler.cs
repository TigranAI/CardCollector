using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Payments;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.PreCheckoutQueryHandler
{
    [Attributes.PreCheckoutQueryHandler]
    public abstract class PreCheckoutQueryHandler : HandlerModel
    {
        protected readonly PreCheckoutQuery PreCheckoutQuery;

        public static readonly ICollection<Type> Commands;
        
        static PreCheckoutQueryHandler()
        {
            Commands = new LinkedList<Type>();
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes())
            {
                if (type == typeof(PreCheckoutQueryHandler)) continue;
                if (Attribute.IsDefined(type, typeof(Attributes.PreCheckoutQueryHandler)))
                    Commands.Add(type);
            }
        }
        
        public static async Task<HandlerModel> Factory(Update update)
        {
            
            var context = new BotDatabaseContext();
            var user = await context.Users.FindUser(update.PreCheckoutQuery!.From);
            if (user.IsBlocked) return new IgnoreHandler(user, context);
            
            foreach (var handlerType in Commands)
            {
                var handler = (HandlerModel?) Activator.CreateInstance(handlerType, user, context, update.PreCheckoutQuery);
                if (handler != null && handler.Match()) return handler;
            }
            
            return new IgnoreHandler(user, context);
        }

        public override bool Match()
        {
            return CommandText == PreCheckoutQuery.InvoicePayload;
        }

        protected PreCheckoutQueryHandler(User user, BotDatabaseContext context, PreCheckoutQuery preCheckoutQuery) : base(user, context)
        {
            PreCheckoutQuery = preCheckoutQuery;
        }
    }
}