using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Commands.CallbackQueryHandler.Others;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQueryHandler.Collection
{
    [SkipCommand]
    public class DeleteCombine : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.delete_combine;

        protected override async Task Execute()
        {
            var stickerId = long.Parse(CallbackQuery.Data!.Split('=')[1]);
            var module = User.Session.GetModule<CombineModule>();
            module.CombineList.RemoveAll(item => item.Item1.Id == stickerId);
            if (module.CombineList.Count == 0)
                await new Back().Init(User, Context, new Update() {CallbackQuery = CallbackQuery}).PrepareAndExecute();
            else await User.Messages.EditMessage(module.ToString(), Keyboard.GetCombineKeyboard(module));
        }
    }
}