using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class AddForSaleSticker : CallbackQueryCommand
    {
        protected override string CommandText => Command.add_for_sale_sticker;
        public override async Task Execute()
        {
            User.Session.State = UserState.LoadForSaleSticker;
            var page = int.Parse(CallbackData.Split('=')[1]);
            var packs = await PacksDao.GetAll();
            var totalCount = packs.Count;
            packs = packs.GetRange((page - 1) * 10, packs.Count >= page * 10 ? 10 : packs.Count % 10);
            if (packs.Count == 0)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.page_not_found);
            else
                await MessageController.EditMessage(User, Messages.select_pack,
                    Keyboard.GetPacksKeyboard(packs, Command.select_for_sale_pack, 
                        Keyboard.GetPagePanel(page, totalCount, CommandText)));
        }

        public AddForSaleSticker() { }
        public AddForSaleSticker(UserEntity user, Update update) : base(user, update) { }
    }
}