using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class TierCallback : CallbackQuery
    {
        protected override string CommandText => Command.tier;
        public override async Task Execute()
        {
            await MessageController.EditMessage(User, CallbackMessageId, Messages.choose_tier, Keyboard.TierOptions);
        }
        
        public TierCallback() { }
        public TierCallback(UserEntity user, Update update) : base(user, update) { }
    }
}