using System.Threading.Tasks;
using CardCollector.Commands.MessageHandler.Collection;
using CardCollector.Commands.MessageHandler.Menu;
using CardCollector.Commands.MessageHandler.Shop;
using CardCollector.DataBase;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Others
{
    public class Back : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.back;

        protected override async Task Execute()
        {
            EnterEmoji.RemoveFromQueue(User.Id);
            EnterGemsExchange.RemoveFromQueue(User.Id);
            EnterGemsPrice.RemoveFromQueue(User.Id);
            if (User.Session.TryGetPreviousMenu(out var menu))
                await menu.BackToThis(User.Session);
            else
            {
                await User.Session.EndSession();
                await User.Messages.ClearChat(User);
                await User.Messages.SendMenu(User);
            }
        }

        public Back(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery)
        {
        }
    }
}