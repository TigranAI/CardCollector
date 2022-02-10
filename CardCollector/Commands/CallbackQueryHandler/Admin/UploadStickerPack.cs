using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.Resources;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Admin
{
    public class UploadStickerPack : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.upload_stickerpack;
        protected override bool AddToStack => true;

        protected override async Task Execute()
        {
            User.Session.State = UserState.UploadSticker;
            await User.Messages.EditMessage(User, Messages.upload_your_stickers, Keyboard.BackKeyboard);
        }

        public override bool Match()
        {
            return base.Match() && User.PrivilegeLevel >= PrivilegeLevel.Artist;
        }

        public UploadStickerPack(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user,
            context, callbackQuery)
        {
        }
    }
}