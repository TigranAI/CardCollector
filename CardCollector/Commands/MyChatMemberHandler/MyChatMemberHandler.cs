using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using CardCollector.Attributes.Handlers;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.MyChatMemberHandler
{
    [MyChatMemberHandler]
    public abstract class MyChatMemberHandler : HandlerModel
    {
        protected override string CommandText => "";
        protected readonly ChatMemberUpdated ChatMemberUpdated;

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


        private async Task BlockChat()
        {
            if (ChatMemberUpdated.Chat.Type is (ChatType.Group or ChatType.Supergroup or ChatType.Channel))
            {
                
            }
        }

        public static async Task<HandlerModel> Factory(Update update)
        {
            var context = new BotDatabaseContext();
            var user = await context.Users.FindUser(update.MyChatMember!.From);

            user.InitSession();

            foreach (var handlerType in Commands)
            {
                var handler = (HandlerModel?) Activator.CreateInstance(handlerType, user, context, update.MyChatMember);
                if (handler != null && handler.Match()) return handler;
            }

            return new IgnoreHandler(user, context);
        }

        protected MyChatMemberHandler(User user, BotDatabaseContext context, ChatMemberUpdated member) : base(user,
            context)
        {
            ChatMemberUpdated = member;
        }
    }
}