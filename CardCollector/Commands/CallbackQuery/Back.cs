using System.Threading.Tasks;
using CardCollector.Commands.Message;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class Back : CallbackQueryCommand
    {
        protected override string CommandText => Command.back;
        
        public override async Task Execute()
        {
            EnterEmoji.RemoveFromQueue(User.Id);
            EnterGemsExchange.RemoveFromQueue(User.Id);
            EnterGemsPrice.RemoveFromQueue(User.Id);
            if (User.Session.TryGetPreviousMenu(out var menu))
                await menu.BackToThis(User.Session);
            else await User.Session.EndSession();
        }
        
        public Back() { }
        public Back(UserEntity user, Update update) : base(user, update) { }
    }
}