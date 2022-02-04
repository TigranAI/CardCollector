using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class OpenAuthorPackMenu : CallbackQueryHandler
    {
        protected override string CommandText => Command.open_author_pack_menu;
        protected override bool AddToStack => true;
        protected override bool ClearStickers => true;

        public override async Task Execute()
        {
            var packs = (await UserPacksDao.GetUserPacks(User.Id))
                .Where(item => item.Count > 0 && item.PackId != 1).ToList();
            if (packs.Count == 0)
            {
                if (User.Session.PreviousCommandType == typeof(OpenPack))
                {
                    User.Session.PopLast();
                    await new Back(User, Update).Execute();
                }
                else await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.packs_count_zero, true);
            }
            else
            {
                var offset = int.Parse(CallbackData.Split('=')[1]);
                var totalCount = packs.Count;
                packs = packs.Skip(offset).Take(10).ToList();
                if (packs.Count == 0)
                    await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.page_not_found);
                else
                    await MessageController.EditMessage(User, Messages.choose_author,
                        await Keyboard.GetUserPacksKeyboard(packs, offset, totalCount));
            }
        }

        public OpenAuthorPackMenu(UserEntity user, Update update) : base(user, update) { }
    }
}