using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class ConfirmPreview : CallbackQueryCommand
    {
        protected override string CommandText => Command.confirm_preview;
        protected override bool ClearStickers => true;

        public override async Task Execute()
        {
            User.Session.State = UserState.Default;
            var module = User.Session.GetModule<UploadPreviewModule>();
            var pack = await PacksDao.GetById(module.PackId);
            pack.Animated = module.Animated;
            pack.StickerPreview = module.StickerId;
            module.Reset();
            await BotDatabase.SaveData();
            await MessageController.EditMessage(User, Messages.set_preview_success, Keyboard.BackKeyboard);
        }

        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            return base.IsMatches(user, update) && user.PrivilegeLevel >= PrivilegeLevel.Programmer &&
                   user.Session.State == UserState.UploadPackPreview;
        }

        public ConfirmPreview()
        {
        }

        public ConfirmPreview(UserEntity user, Update update) : base(user, update)
        {
        }
    }
}