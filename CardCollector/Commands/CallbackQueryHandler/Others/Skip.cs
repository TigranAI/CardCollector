using System.Threading.Tasks;
using CardCollector.Commands.MessageHandler.Admin.Giveaway;
using CardCollector.Database;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Others
{
    public class Skip : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.skip;
        protected override async Task Execute()
        {
            var commandName = CallbackQuery.Data!.Split("=")[1];
            if (commandName == typeof(EnterSendDatetime).Name) await EnterSendDatetime.Skip(User, Context);
            else if (commandName == typeof(EnterButtonText).Name) await EnterButtonText.Skip(User, Context);
            else if (commandName == typeof(SendGiveawayImage).Name) await SendGiveawayImage.Skip(User, Context);
        }
        
        public Skip(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery)
        {
        }
    }
}