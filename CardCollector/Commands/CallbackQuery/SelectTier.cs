using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class SelectTier : CallbackQueryHandler
    {
        protected override string CommandText => Command.tier;

        public override async Task Execute()
        {
            await MessageController.EditMessage(User, Messages.choose_tier, Keyboard.TierOptions);
        }
        
        public SelectTier(UserEntity user, Update update) : base(user, update) { }
    }
}