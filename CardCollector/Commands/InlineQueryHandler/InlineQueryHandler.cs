using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.InlineQueryHandler
{
    [Attributes.Handlers.InlineQueryHandler]
    public abstract class InlineQueryHandler : HandlerModel
    {
        protected InlineQuery InlineQuery;
        protected override string CommandText => "";

        public static readonly ICollection<Type> Commands;
        
        static InlineQueryHandler()
        {
            Commands = new LinkedList<Type>();
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes())
            {
                if (type == typeof(InlineQueryHandler)) continue;
                if (Attribute.IsDefined(type, typeof(Attributes.Handlers.InlineQueryHandler)))
                    Commands.Add(type);
            }
        }

        public static async Task<HandlerModel> Factory(Update update)
        {
            var context = new BotDatabaseContext();
            var user = await context.Users.FindUser(update.InlineQuery!.From);
            if (user.IsBlocked) return new IgnoreHandler();
            
            user.InitSession();

            foreach (var handlerType in Commands)
            {
                var handler = (HandlerModel?) Activator.CreateInstance(handlerType);
                if (handler != null && handler.Init(user, context, update).Match()) return handler;
            }
            
            return new IgnoreHandler();
        }

        public override HandlerModel Init(User user, BotDatabaseContext context, Update update)
        {
            InlineQuery = update.InlineQuery!;
            return base.Init(user, context, update);
        }
    }
}