using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.Resources;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Shop
{
    public class PackInfo : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.pack_info;

        protected override async Task Execute()
        {
            await User.Messages.EditMessage(User, Messages.pack_info, Keyboard.BackKeyboard);
        }

        public PackInfo(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery) { }
    }
}