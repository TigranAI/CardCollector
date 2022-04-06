using System.Threading.Tasks;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.MessageHandler.UrlCommands
{
    public class ClaimGiveaway : MessageUrlHandler
    {
        protected override string CommandText => MessageUrlCommands.claim_giveaway;

        protected override async Task Execute()
        {
            var giveaway = await Context.ChannelGiveaways.FindById(int.Parse(StartData[1]));
            if (giveaway == null || giveaway.IsEnded())
                await User.Messages.EditMessage(User, Messages.giveaway_now_ended, Keyboard.BackKeyboard);
            else if (giveaway.IsAwarded(User.Id))
                await User.Messages.EditMessage(User, Messages.you_are_now_awarded, Keyboard.BackKeyboard);
            else
            {
                await giveaway.Claim(User, Context);
                await User.Messages.EditMessage(User,
                    string.Format(Messages.you_got_from_this_giveaway, giveaway.PrizeText()), Keyboard.BackKeyboard);
            }
        }
    }
}