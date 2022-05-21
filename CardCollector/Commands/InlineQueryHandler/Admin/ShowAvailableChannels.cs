using System.Linq;
using System.Threading.Tasks;
using CardCollector.Commands.ChosenInlineResultHandler;
using CardCollector.Extensions;
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
            var offset = Offset.Of(InlineQuery);
            var length = 0;
            
            var results = (await Context.TelegramChats
                .Where(item =>
                    !item.IsBlocked
                    && item.ChatType == ChatType.Channel
                    && item.Title.Contains(InlineQuery.Query))
                .And(list => length = list.Count())
                .ToListAsync())
                .ToTelegramResults(ChosenInlineResultCommands.set_giveaway_channel, offset);
            
            await AnswerInlineQuery(User, InlineQuery.Id, results, offset.GetNext(length));
        }

        public override bool Match()
        {
            return User.Session.State is UserState.CreateGiveaway;
        }
    }
}