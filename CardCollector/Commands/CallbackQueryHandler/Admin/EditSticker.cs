using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Attributes.Menu;
using CardCollector.DataBase;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Admin
{
    [MenuPoint]
    public class EditSticker : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.edit_sticker;
        protected override bool ClearStickers => true;
        protected override async Task Execute()
        {
            User.Session.State = UserState.EditSticker;
            User.Session.GetModule<AdminModule>().SelectedPackId = int.Parse(CallbackQuery.Data!.Split('=')[1]);
            await User.Messages.EditMessage(User, Messages.select_sticker, Keyboard.ShowStickers);
        }

        public override bool Match()
        {
            return base.Match() && User.PrivilegeLevel >= PrivilegeLevel.Programmer;
        }

        public EditSticker(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery)
        {
        }
    }
}