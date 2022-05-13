using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.MyChatMemberHandler
{
    public abstract class MyChatMemberHandler : HandlerModel
    {
        protected override string CommandText => "";
        protected ChatMemberUpdated ChatMemberUpdated;

        public static readonly ICollection<Type> Commands = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type => type.IsSubclassOf(typeof(MyChatMemberHandler)) && !type.IsAbstract)
            .ToList();

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