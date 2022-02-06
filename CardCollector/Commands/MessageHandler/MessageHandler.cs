using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.MessageHandler
{
    [Attributes.MessageHandler]
    public abstract class MessageHandler : HandlerModel
    {
        protected Message Message;
        
        private static ICollection<Type> Commands;

        static MessageHandler()
        {
            Commands = new LinkedList<Type>();
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes())
            {
                if (type == typeof(MessageHandler)) continue;
                if (Attribute.IsDefined(type, typeof(Attributes.MessageHandler)))
                    Commands.Add(type);
            }
        }

        public static async Task<HandlerModel> Factory(Update update)
        {
            var context = new BotDatabaseContext();
            var user = await context.Users.FindUserWithSession(update.Message!.From!);
            if (user.IsBlocked) return new IgnoreHandler(user, context);
            
            if (update.Message?.From?.Username == AppSettings.NAME)
            {
                await Bot.Client.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId);
                return new IgnoreHandler(user, context);
            }

            
            if (update.Message!.Chat.Type is ChatType.Private && update.Message.Text != Text.start)
                await MessageController.DeleteMessage(user, update.Message.MessageId);

            foreach (var handlerType in Commands)
            {
                var handler = (HandlerModel?) Activator.CreateInstance(handlerType, user, context, update.Message);
                if (handler != null && handler.Match()) return handler;
            }

            return new IgnoreHandler(user, context);
        }

        public override bool Match()
        {
            return Message.Type == MessageType.Text && Message.Text == CommandText;
        }

        protected MessageHandler(User user, BotDatabaseContext context, Message message) : base(user, context)
        {
            Message = message;
        }
    }
}