using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class BackToCombine : CallbackQuery
    {
        protected override string CommandText => Command.back_to_combine;
        public override async Task Execute()
        {
            await User.ClearChat();
            var message = await MessageController.SendMessage(User, User.Session.GetCombineMessage(), 
                Keyboard.GetCombineKeyboard(User.Session));
            User.Session.Messages.Add(message.MessageId);
        }

        public BackToCombine() { }
        public BackToCombine(UserEntity user, Update update) : base(user, update) { }
    }
}