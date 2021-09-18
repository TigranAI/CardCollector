using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class SortCallback : CallbackQuery
    {
        protected override string CommandText => Command.sort;
        public override async Task Execute()
        {
            await MessageController.EditMessage(User, CallbackMessageId, Messages.choose_sort, Keyboard.SortOptions);
        }

        public SortCallback() { }
        public SortCallback(UserEntity user, Update update) : base(user, update) { }
    }
}