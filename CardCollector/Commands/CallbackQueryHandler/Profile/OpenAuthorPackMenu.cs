using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes.Menu;
using CardCollector.Commands.CallbackQueryHandler.Others;
using CardCollector.Controllers;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQueryHandler.Profile
{
    [MenuPoint]
    public class OpenAuthorPackMenu : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.open_author_pack_menu;
        protected override bool ClearStickers => true;
        private bool _fromOpenPackCommand;

        protected override async Task Execute()
        {
            var packs = User.Packs.Where(item => item.Count > 0 && item.Pack.Id != 1).ToList();
            if (packs.Count == 0)
            {
                await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.packs_count_zero, true);
                if (_fromOpenPackCommand) 
                    await new Back()
                        .Init(User, Context, new Update(){CallbackQuery = CallbackQuery})
                        .PrepareAndExecute();
            }
            else
            {
                var offset = int.Parse(CallbackQuery.Data!.Split('=')[1]);
                var totalCount = packs.Count;
                packs = packs.Skip(offset).Take(10).ToList();
                if (packs.Count == 0)
                    await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.page_not_found);
                else
                    await User.Messages.EditMessage(User, Messages.choose_author,
                        Keyboard.GetUserPacksKeyboard(packs, offset, totalCount));
            }
        }

        public override async Task InitNewContext(long userId)
        {
            await base.InitNewContext(userId);
            _fromOpenPackCommand = true;
        }
    }
}