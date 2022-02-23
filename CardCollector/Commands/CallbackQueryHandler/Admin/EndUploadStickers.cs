using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

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