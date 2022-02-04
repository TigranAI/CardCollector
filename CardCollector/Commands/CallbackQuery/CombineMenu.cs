using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class CombineMenu : CallbackQueryHandler
    {
        protected override string CommandText => "";
        protected override bool AddToStack => true;
        protected override bool ClearStickers => true;

        public override async Task Execute()
        {
            var combineModule = User.Session.GetModule<CombineModule>();
            if (combineModule.CombineCount == 0)
                await new Back(User, Update).Execute();
            else 
                await MessageController.EditMessage(User, combineModule.ToString(), Keyboard.GetCombineKeyboard(combineModule));
        }

        protected internal override bool Match(UserEntity user, Update update) => false;

        public CombineMenu(UserEntity user, Update update) : base(user, update) { }
    }
}