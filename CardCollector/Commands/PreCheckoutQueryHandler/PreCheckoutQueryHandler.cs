using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Payments;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.PreCheckoutQueryHandler
{
    public abstract class PreCheckoutQueryHandler : HandlerModel
    {
        protected PreCheckoutQuery PreCheckoutQuery;

        public static readonly ICollection<Type> Commands = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type => type.IsSubclassOf(typeof(PreCheckoutQueryHandler)) && !type.IsAbstract)
            .ToList();
        
        public static async Task<HandlerModel> Factory(Update update)
        {
            
            var context = new BotDatabaseContext();
            var user = await context.Users.FindUser(update.PreCheckoutQuery!.From);
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
            return CommandText == PreCheckoutQuery.InvoicePayload;
        }

        public override HandlerModel Init(User user, BotDatabaseContext context, Update update)
        {
            PreCheckoutQuery = update.PreCheckoutQuery!;
            return base.Init(user, context, update);
        }
    }
}