using System.Threading.Tasks;
using CardCollector.Commands.Message.TextMessage;
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
            User.Session.SelectedSticker = null;
            EnterEmojiMessage.RemoveFromQueue(User.Id);
            EnterCoinsPriceMessage.RemoveFromQueue(User.Id);
            EnterGemsPriceMessage.RemoveFromQueue(User.Id);
            await User.ClearChat();
        }

        public CancelCallback() { }
        public CancelCallback(UserEntity user, Update update) : base(user, update) { }
    }
}