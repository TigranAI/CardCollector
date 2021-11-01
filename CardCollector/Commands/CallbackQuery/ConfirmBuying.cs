using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class ConfirmBuying : CallbackQueryCommand
    {
        protected override string CommandText => Command.confirm_buying;

        public override async Task Execute()
        {
            var auctionModule = User.Session.GetModule<AuctionModule>();
            var price = auctionModule.Price * auctionModule.Count ;
            if (price > User.Cash.Gems)
                await MessageController.AnswerCallbackQuery(User, Update.CallbackQuery!.Id, Messages.not_enougth_gems, true);
            else
            {
                var text = $"{Messages.confirm_buying}" +
                           $"\n{auctionModule.Count}{Text.items} {Text.per} {price}{Text.gem}" +
                           $"\n{Messages.are_you_sure}";
                await MessageController.EditMessage(User, text, Keyboard.GetConfirmationKeyboard(Command.buy_sticker));
            }
        }

        public ConfirmBuying() { }
        public ConfirmBuying(UserEntity user, Update update) : base(user, update) { }
    }
}