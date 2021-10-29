using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class PackInfo : CallbackQueryCommand
    {
        protected override string CommandText => Command.pack_info;

        public override async Task Execute()
        {
            await MessageController.SendMessage(User, Messages.pack_info, Keyboard.BackKeyboard);
        }

        public PackInfo() { }
        public PackInfo(UserEntity user, Update update) : base(user, update) { }
    }
}