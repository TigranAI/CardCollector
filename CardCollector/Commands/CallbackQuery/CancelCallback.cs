using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class CancelCallback : CallbackQuery
    {
        protected override string CommandText => Command.cancel;
        public override async Task Execute()
        {
            User.Session.State = UserState.Default;
            await User.ClearChat();
        }

        public CancelCallback() { }
        public CancelCallback(UserEntity user, Update update) : base(user, update) { }
    }
}