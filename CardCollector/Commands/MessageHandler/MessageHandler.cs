using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using CardCollector.Attributes.Handlers;
using CardCollector.Controllers;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.MessageHandler
{
    [MessageHandler]
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
                if (type.IsAbstract) continue;
                if (Attribute.IsDefined(type, typeof(MessageHandlerAttribute)))
                    Commands.Add(type);
            }
        }

        public static async Task<HandlerModel> Factory(Update update)
        {
            var context = new BotDatabaseContext();
            var user = await context.Users.FindUser(update.Message!.From!);
            if (user.IsBlocked) return new IgnoreHandler();
            
            if (update.Message?.From?.Username == AppSettings.NAME)
            {
                await Bot.Client.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId);
                return new IgnoreHandler();
            }

            if (update.Message!.Chat.Type is ChatType.Private && update.Message.Text != Text.start)
                await MessageController.DeleteMessage(user.ChatId, update.Message.MessageId);

            if (update.Message.Chat.Type is ChatType.Group or ChatType.Supergroup)
                await GroupController.OnGroupMessageReceived(update.Message.Chat, update.Message.From!);

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
            if (Message.Chat.Type is not (ChatType.Sender or ChatType.Private)) return false;
            if (Message.Type != MessageType.Text) return false;
            return Message.Text == CommandText;
        }

        public override HandlerModel Init(User user, BotDatabaseContext context, Update update)
        {
            Message = update.Message!;
            return base.Init(user, context, update);
        }
    }
}