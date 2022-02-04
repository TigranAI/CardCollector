using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class EditSticker : CallbackQueryHandler
    {
        protected override string CommandText => Command.edit_sticker;
        protected override bool AddToStack => true;
        protected override bool ClearStickers => true;
        public override async Task Execute()
        {
            User.Session.State = UserState.EditSticker;
            var module = User.Session.GetModule<AdminModule>();
            var packId = int.Parse(CallbackData.Split('=')[1]);
            module.SelectedPack = await PacksDao.GetById(packId);
            await MessageController.EditMessage(User, Messages.select_sticker, Keyboard.ShowStickers);
        }

        protected internal override bool Match(UserEntity user, Update update)
        {
            return base.Match(user, update) && user.PrivilegeLevel >= PrivilegeLevel.Programmer;
        }

        public EditSticker(UserEntity user, Update update) : base(user, update)
        {
        }
    }
}