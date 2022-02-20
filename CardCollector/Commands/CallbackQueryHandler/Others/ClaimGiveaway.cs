using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Others
{
    public class ClaimGiveaway : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.claim_giveaway;

        protected override async Task Execute()
        {
            var giveawayId = int.Parse(CallbackQuery.Data!.Split("=")[1]);
            var giveaway = await Context.ChannelGiveaways.FindById(giveawayId);
            if (giveaway == null || giveaway.IsEnded())
                await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.gievaway_now_ended, true);
            else if (giveaway.IsAwarded(User.Id))
                await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.you_are_now_awarded, true);
            else
            {
                await giveaway.Claim(User, Context);
                await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id,
                    string.Format(Messages.you_got_from_this_giveaway, giveaway.PrizeText()), true);
            }
        }

        public ClaimGiveaway(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context,
            callbackQuery)
        {
        }
    }
}