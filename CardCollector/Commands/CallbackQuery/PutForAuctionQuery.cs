using System.Linq;
using System.Threading.Tasks;
using CardCollector.Commands.Message.TextMessage;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class PutForAuctionQuery : CallbackQuery
    {
        protected override string CommandText => Command.sell_on_auction;

        public override async Task Execute()
        {
            await MessageController.AnswerCallbackQuery(User, CallbackQueryId,
                Messages.comission_warning, true);
            await User.ClearChat();
            var module = User.Session.GetModule<CollectionModule>();
            var priceList = (await AuctionController.GetPriceList(module.SelectedSticker.Id)).ToList();
            var lowerPrice = priceList.Count > 0 ? priceList.Min() : 0;
            var message = await MessageController.SendMessage(User,
                $"{Messages.current_price} {module.SellPrice}{Text.gem}" +
                $"\n{Messages.lower_price} {lowerPrice}{Text.gem}" +
                $"\n{Messages.enter_your_gems_price} {Text.gem}:", Keyboard.AuctionPutCancelKeyboard);
            EnterGemsPriceMessage.AddToQueue(User.Id, message.MessageId);
            User.Session.Messages.Add(message.MessageId);
        }

        public PutForAuctionQuery() { }
        public PutForAuctionQuery(UserEntity user, Update update) : base(user, update) { }
    }
}