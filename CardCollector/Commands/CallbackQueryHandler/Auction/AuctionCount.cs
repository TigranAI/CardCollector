using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Auction
{
    public class AuctionCount : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.count;

        protected override async Task Execute()
        {
            var module = User.Session.GetModule<AuctionModule>();
            var auction = await Context.Auctions.FindById(module.SelectedAuctionId);
            if (CallbackQuery.Data!.Contains(Text.plus))
            {
                if (module.Count < auction?.Count) module.Count++;
                else await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.cant_change_count);
            }
            else if (CallbackQuery.Data!.Contains(Text.minus))
            {
                if (module.Count > 1) module.Count--;
                else await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.cant_change_count);
            }

            await MessageController.EditReplyMarkup(User, CallbackQuery.Message!.MessageId,
                Keyboard.GetAuctionProductKeyboard(auction!, User, module.Count));
        }

        public override bool Match()
        {
            return base.Match() && User.Session.State is UserState.AuctionMenu;
        }

        public AuctionCount(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery)
        {
        }
    }
}