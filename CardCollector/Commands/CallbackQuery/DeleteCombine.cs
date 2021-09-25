using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class DeleteCombine : CallbackQuery
    {
        protected override string CommandText => Command.delete_combine;
        public override async Task Execute()
        {
            var hash = CallbackData.Split('=')[1];
            User.Session.CombineList.Remove(hash);
            if (User.Session.CombineList.Count == 0)
                await new BackToFiltersMenu(User, Update).Execute();
            else await MessageController.EditMessage(User, CallbackMessageId, 
                User.Session.GetCombineMessage(), Keyboard.GetCombineKeyboard(User.Session));
        }

        public DeleteCombine() { }
        public DeleteCombine(UserEntity user, Update update) : base(user, update) { }
    }
}