using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class BuyAuthorPackMenu : CallbackQueryCommand
    {
        protected override string CommandText => Command.buy_author_pack_menu;
        protected override bool AddToStack => true;
        
        public override async Task Execute()
        {
            var page = int.Parse(CallbackData.Split('=')[1]);
            var packs = await PacksDao.GetAll();
            var totalCount = packs.Count;
            packs = packs.GetRange((page - 1) * 10, packs.Count >= page * 10 ? 10 : packs.Count % 10);
            if (packs.Count == 0)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.page_not_found);
            else
                await MessageController.EditMessage(User, Messages.choose_author,
                    Keyboard.GetShopPacksKeyboard(packs, Keyboard.GetPagePanel(page, totalCount, CommandText)));
        }

        public BuyAuthorPackMenu() { }
        public BuyAuthorPackMenu(UserEntity user, Update update) : base(user, update) { }
    }
}