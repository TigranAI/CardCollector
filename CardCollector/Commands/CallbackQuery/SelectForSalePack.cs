using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class SelectForSalePack : CallbackQueryCommand
    {
        protected override string CommandText => Command.select_for_sale_pack;
        protected override bool AddToStack => true;
        protected override bool ClearStickers => true;
        public override async Task Execute()
        {
            User.Session.State = UserState.LoadForSaleSticker;
            var packId = int.Parse(CallbackData.Split('=')[1]);
            var packInfo = await PacksDao.GetById(packId);
            var module = User.Session.GetModule<AdminModule>();
            module.SelectedPack = packInfo;
            await MessageController.EditMessage(User, Messages.choose_sticker, Keyboard.ShowStickers);
        }

        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            return base.IsMatches(user, update) && User.PrivilegeLevel >= PrivilegeLevel.Programmer;
        }

        public SelectForSalePack() { }
        public SelectForSalePack(UserEntity user, Update update) : base(user, update) { }
    }
}