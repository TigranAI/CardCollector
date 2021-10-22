using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class ClearChat : CallbackQueryCommand
    {
        protected override string CommandText => Command.clear_chat;
        public override async Task Execute()
        {
            await User.ClearChat();
        }

        public ClearChat() { }
        public ClearChat(UserEntity user, Update update) : base(user, update) { }
    }
}