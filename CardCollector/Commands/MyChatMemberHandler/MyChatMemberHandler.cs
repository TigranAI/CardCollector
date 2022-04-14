using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using CardCollector.Attributes.Handlers;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.MyChatMemberHandler
{
    [MyChatMemberHandler]
    public abstract class MyChatMemberHandler : HandlerModel
    {
        protected override string CommandText => "";
        protected ChatMemberUpdated ChatMemberUpdated;

        public static readonly ICollection<Type> Commands;

        static MyChatMemberHandler()
        {
            Commands = new LinkedList<Type>();
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes())
            {
                if (type == typeof(MyChatMemberHandler)) continue;
                if (Attribute.IsDefined(type, typeof(MyChatMemberHandlerAttribute)))
                    Commands.Add(type);
            }
        }

        public static async Task<HandlerModel> Factory(Update update)
        {
            var context = new BotDatabaseContext();
            var user = await context.Users.FindUser(update.MyChatMember!.From);

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
            ChatMemberUpdated = update.MyChatMember!;
            return base.Init(user, context, update);
        }
    }
}