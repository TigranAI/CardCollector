using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class DeleteCombine : CallbackQuery
    {
        protected override string CommandText => Command.delete_combine;
        public override async Task Execute()
        {
            var sticker = await StickerDao.GetStickerByHash(CallbackData.Split('=')[1]);
            var module = User.Session.GetModule<CombineModule>();
            module.CombineList.Remove(sticker);
            if (module.CombineList.Count == 0)
                await new BackToFiltersMenu(User, Update).Execute();
            else await MessageController.EditMessage(User, CallbackMessageId, module.ToString(), Keyboard.GetCombineKeyboard(module));
        }

        public DeleteCombine() { }
        public DeleteCombine(UserEntity user, Update update) : base(user, update) { }
    }
}