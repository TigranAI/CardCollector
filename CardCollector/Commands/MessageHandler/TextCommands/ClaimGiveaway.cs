using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.MessageHandler.TextCommands
{
    public class ClaimGiveaway : MessageHandler
    {
        protected override string CommandText => MessageCommands.claim_giveaway;

        protected override async Task Execute()
        {
            var giveawayId = int.Parse(Message.Text!.Split(' ')[1].Split('=')[1]);
            var giveaway = await Context.ChannelGiveaways.FindById(giveawayId);
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

        public override bool Match()
        {
            if (Message.Type != MessageType.Text) return false;
            var data = Message.Text!.Split(' ');
            if (data[0] != MessageCommands.start) return false;
            if (data.Length < 2) return false;
            var command = data[1].Split('=')[0];
            return command == CommandText;
        }

        public ClaimGiveaway(User user, BotDatabaseContext context, Message message) : base(user, context, message)
        {
        }
    }
}