using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class ChoosePack : CallbackQueryCommand
    {
        /* Command syntax select_pack=<target command>=<offset> */
        protected override string CommandText => Command.choose_pack;
        protected override bool AddToStack => true;
        protected override bool ClearStickers => true;

        public override async Task Execute()
        {
            var data = CallbackData.Split('=');
            var offset = int.Parse(data[2]);
            var targetCommand = data[1];
            var packs = await PacksDao.GetNext(offset, 10);
            if (packs.Count == 0)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.page_not_found);
            else
                await MessageController.EditMessage(User, Messages.choose_author,
                    Keyboard.GetPacksKeyboard(packs, offset, await PacksDao.GetCount(), targetCommand));
        }

        public ChoosePack()
        {
        }

        public ChoosePack(UserEntity user, Update update) : base(user, update)
        {
        }
    }
}