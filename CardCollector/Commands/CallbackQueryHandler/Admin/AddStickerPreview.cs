using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Admin
{
    public class AddStickerPreview : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.add_sticker_preview;
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            User.Session.State = UserState.UploadPackPreview;
            var packId = int.Parse(CallbackQuery.Data!.Split('=')[1]);
            User.Session.GetModule<AdminModule>().SelectedPackId = packId;
            await User.Messages.EditMessage(User, Messages.please_upload_preview, Keyboard.BackKeyboard);
        }

        public override bool Match()
        {
            return base.Match() && User.PrivilegeLevel >= PrivilegeLevel.Programmer;
        }

        public AddStickerPreview(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery)
        {
        }
    }
}