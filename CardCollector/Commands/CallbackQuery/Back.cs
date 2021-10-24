using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class Back : CallbackQueryCommand
    {
        protected override string CommandText => Command.back;
        protected override bool ClearMenu => false;
        protected override bool AddToStack => false;
        
        public override async Task Execute()
        {
            await User.ClearChat();
            if (User.Session.TryGetPreviousMenu(out var menu))
                await menu.BackToThis(User.Session);
            else await User.Session.EndSession();
        }
        
        public Back() { }
        public Back(UserEntity user, Update update) : base(user, update) { }
    }
}