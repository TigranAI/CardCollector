using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class SelectSort : CallbackQueryCommand
    {
        protected override string CommandText => Command.sort;

        public override async Task Execute()
        {
            await MessageController.EditMessage(User, Messages.choose_sort, Keyboard.SortOptions);
        }

        public SelectSort() { }
        public SelectSort(UserEntity user, Update update) : base(user, update) { }
    }
}