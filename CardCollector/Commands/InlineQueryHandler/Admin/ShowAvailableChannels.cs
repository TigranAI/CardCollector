using System.Linq;
using System.Threading.Tasks;
using CardCollector.Commands.ChosenInlineResultHandler;
using CardCollector.Controllers;
using CardCollector.Others;
using CardCollector.Resources.Enums;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.InlineQueryHandler.Admin
{
    public class ShowAvailableChannels : InlineQueryHandler
    {
        protected override async Task Execute()
        {
            var offset = int.Parse(InlineQuery.Offset == "" ? "0" : InlineQuery.Offset);
            var channels = await Context.TelegramChats
                .Where(item => item.IsBlocked == false && item.ChatType == ChatType.Channel)
                .Skip(offset)
                .Take(50)
                .ToListAsync();
            var newOffset = offset + 50 > channels.Count ? "" : (offset + 50).ToString();
            var results = channels
                .ToTelegramResults(ChosenInlineResultCommands.set_giveaway_channel, offset);
            await MessageController.AnswerInlineQuery(User, InlineQuery.Id, results, newOffset);
        }

        public override bool Match()
        {
            return User.Session.State is UserState.CreateGiveaway;
        }
    }
}