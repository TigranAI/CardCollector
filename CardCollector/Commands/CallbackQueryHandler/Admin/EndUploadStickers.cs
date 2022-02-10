using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.Resources;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Admin
{
    public class EndUploadStickers : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.end_sticker_upload;

        protected override async Task Execute()
        {
            User.Session.State = UserState.UploadFile;
            await User.Messages.EditMessage(User, Messages.upload_your_file, Keyboard.BackKeyboard);
        }

        public EndUploadStickers(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user,
            context, callbackQuery)
        {
        }
    }
}