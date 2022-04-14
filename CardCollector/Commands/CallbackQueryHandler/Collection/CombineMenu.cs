using System.Threading.Tasks;
using CardCollector.Attributes.Menu;
using CardCollector.Commands.CallbackQueryHandler.Others;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQueryHandler.Collection
{
    [MenuPoint]
    public abstract class CombineMenu : CallbackQueryHandler
    {
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            var combineModule = User.Session.GetModule<CombineModule>();
            if (combineModule.CombineCount == 0)
                await new Back().Init(User, Context, new Update() {CallbackQuery = CallbackQuery}).PrepareAndExecute();
            else 
                await User.Messages.EditMessage(User, combineModule.ToString(), Keyboard.GetCombineKeyboard(combineModule));
        }
    }
}