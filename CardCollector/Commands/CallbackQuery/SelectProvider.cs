using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class SelectProvider : CallbackQueryCommand
    {
        protected override string CommandText => Command.select_provider;
        public override async Task Execute()
        {
            await MessageController.EditMessage(User, "Выберите платежную систему", Keyboard.ProviderKeyboard);
        }

        public SelectProvider()
        {
        }

        public SelectProvider(UserEntity user, Update update) : base(user, update)
        {
        }
    }
}