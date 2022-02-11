using System.Threading.Tasks;
using CardCollector.Attributes.Menu;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Others
{
    [MenuPoint]
    public class ChoosePack : CallbackQueryHandler
    {
        /* Command syntax select_pack=<target command>=<offset> */
        protected override string CommandText => CallbackQueryCommands.choose_pack;
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            var data = CallbackQuery.Data!.Split('=');
            var offset = int.Parse(data[2]);
            var targetCommand = data[1];
            var packs = await Context.Packs.FindNextSkipRandom(offset, 10);
            if (packs.Count == 0)
                await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.page_not_found);
            else
                await User.Messages.EditMessage(User, Messages.choose_author,
                    Keyboard.GetPacksKeyboard(packs, offset, await Context.Packs.GetCount(), targetCommand));
        }

        public ChoosePack(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery)
        {
        }
    }
}