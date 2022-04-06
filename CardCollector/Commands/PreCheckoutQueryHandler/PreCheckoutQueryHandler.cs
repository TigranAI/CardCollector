using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Payments;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.PreCheckoutQueryHandler
{
    [Attributes.Handlers.PreCheckoutQueryHandler]
    public abstract class PreCheckoutQueryHandler : HandlerModel
    {
        protected PreCheckoutQuery PreCheckoutQuery;

        public static readonly ICollection<Type> Commands;
        
        static PreCheckoutQueryHandler()
        {
            Commands = new LinkedList<Type>();
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes())
            {
                if (type == typeof(PreCheckoutQueryHandler)) continue;
                if (Attribute.IsDefined(type, typeof(Attributes.Handlers.PreCheckoutQueryHandler)))
                    Commands.Add(type);
            }
        }
        
        public static async Task<HandlerModel> Factory(Update update)
        {
            
            var context = new BotDatabaseContext();
            var user = await context.Users.FindUser(update.PreCheckoutQuery!.From);
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
            return CommandText == PreCheckoutQuery.InvoicePayload;
        }

        public override HandlerModel Init(User user, BotDatabaseContext context, Update update)
        {
            PreCheckoutQuery = update.PreCheckoutQuery!;
            return base.Init(user, context, update);
        }
    }
}