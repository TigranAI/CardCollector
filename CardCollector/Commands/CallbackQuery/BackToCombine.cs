using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class BackToCombine : CallbackQueryCommand
    {
        protected override string CommandText => Command.back_to_combine;
        public override async Task Execute()
        {
            await User.ClearChat();
            var combineModule = User.Session.GetModule<CombineModule>();
            var message = await MessageController.SendMessage(User, combineModule.ToString(), Keyboard.GetCombineKeyboard(combineModule));
            User.Session.Messages.Add(message.MessageId);
        }

        public BackToCombine() { }
        public BackToCombine(UserEntity user, Update update) : base(user, update) { }
    }
}