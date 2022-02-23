using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Attributes.Menu;
using CardCollector.Database;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Admin
{
    [MenuPoint]
    public class UploadStickerPack : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.upload_stickerpack;

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