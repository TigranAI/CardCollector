using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class DeleteCombine : CallbackQueryHandler
    {
        protected override string CommandText => Command.delete_combine;

        public override async Task Execute()
        {
            User.Session.UndoCurrentCommand();
            var sticker = await StickerDao.GetByHash(CallbackData.Split('=')[1]);
            var module = User.Session.GetModule<CombineModule>();
            module.CombineList.Remove(sticker);
            if (module.CombineList.Count == 0)
                await new Back(User, Update).PrepareAndExecute();
            else await MessageController.EditMessage(User, module.ToString(), Keyboard.GetCombineKeyboard(module));
        }

        public DeleteCombine(UserEntity user, Update update) : base(user, update) { }
    }
}