using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.Database.EntityDao;
using CardCollector.Extensions;
using CardCollector.Extensions.Database.Entity;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;

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
                await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.giveaway_now_ended, true);
            else if (giveaway.IsAwarded(User.Id))
                await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.you_are_now_awarded, true);
            else
            {
                await giveaway.Claim(User, Context);
                await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id,
                    string.Format(Messages.you_got_from_this_giveaway, giveaway.PrizeText()), true);

                await User.Stickers
                    .Where(sticker => sticker.Sticker.ExclusiveTask is ExclusiveTask.ClaimGiveaway)
                    .Apply(sticker => sticker.DoExclusiveTask());
            }
        }
    }
}