using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class OpenAuthorPackMenu : CallbackQueryCommand
    {
        protected override string CommandText => Command.open_author_pack_menu;
        protected override bool ClearMenu => false;
        protected override bool AddToStack => true;

        public override async Task Execute()
        {
            var packs = (await UserPacksDao.GetUserPacks(User.Id))
                .Where(item => item.Count > 0 && item.PackId != 1).ToList();
            if (packs.Count == 0)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.packs_count_zero, true);
            else
            {
                var page = int.Parse(CallbackData.Split('=')[1]);
                var totalCount = packs.Count;
                packs = packs.GetRange((page - 1) * 10, packs.Count >= page * 10 ? 10 : packs.Count % 10);
                if (packs.Count == 0)
                    await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.page_not_found);
                else
                    await MessageController.EditMessage(User, Messages.choose_author,
                        await Keyboard.GetUserPacksKeyboard(packs, Keyboard.GetPagePanel(page, totalCount, CommandText)));
            }
        }

        public OpenAuthorPackMenu() { }
        public OpenAuthorPackMenu(UserEntity user, Update update) : base(user, update) { }
    }
}