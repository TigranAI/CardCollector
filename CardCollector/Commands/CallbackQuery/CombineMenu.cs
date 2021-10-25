using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class CombineMenu : CallbackQueryCommand
    {
        protected override string CommandText => "";
        protected override bool ClearMenu => false;
        protected override bool AddToStack => true;

        public override async Task Execute()
        {
            var combineModule = User.Session.GetModule<CombineModule>();
            await User.ClearChat();
            await MessageController.EditMessage(User, combineModule.ToString(), Keyboard.GetCombineKeyboard(combineModule));
        }

        protected internal override bool IsMatches(UserEntity user, Update update) => false;

        public CombineMenu(UserEntity user, Update update) : base(user, update) { }
    }
}