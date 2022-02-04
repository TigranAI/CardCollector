using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message
{
    public class Help : MessageHandler
    {
        protected override string CommandText => Text.help;
        public override async Task Execute()
        {
            await MessageController.EditMessage(User, Messages.help);
        }

        public Help(UserEntity user, Update update) : base(user, update) { }
    }
}