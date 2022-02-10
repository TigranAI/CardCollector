using System.Threading.Tasks;
using CardCollector.Commands.CallbackQueryHandler.Others;
using CardCollector.DataBase;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Collection
{
    public class DeleteCombine : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.delete_combine;

        protected override async Task Execute()
        {
            User.Session.UndoCurrentCommand();
            var stickerId = long.Parse(CallbackQuery.Data!.Split('=')[1]);
            var module = User.Session.GetModule<CombineModule>();
            module.CombineList.RemoveAll(item => item.Item1.Id == stickerId);
            if (module.CombineList.Count == 0)
                await new Back(User, Context, CallbackQuery).PrepareAndExecute();
            else await User.Messages.EditMessage(User, module.ToString(), Keyboard.GetCombineKeyboard(module));
        }

        public DeleteCombine(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery) { }
    }
}