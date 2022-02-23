using System.Linq;
using System.Threading.Tasks;
using CardCollector.Commands.ChosenInlineResultHandler;
using CardCollector.Controllers;
using CardCollector.Database;
using CardCollector.Others;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.InlineQueryHandler.Group
{
    public class ShowBetStickers : InlineQueryHandler
    {
        protected override string CommandText => InlineQueryCommands.roulette;

        protected override async Task Execute()
        {
            var query = InlineQuery.Query.Substring(CommandText.Length).TrimStart();
            var stickersList = User.Stickers
                .Where(item => item.Count > 0 && item.Sticker.Contains(query))
                .ToList();
            var offset = int.Parse(InlineQuery.Offset == "" ? "0" : InlineQuery.Offset);
            var newOffset = offset + 50 > stickersList.Count() ? "" : (offset + 50).ToString();
            var results = stickersList
                .ToTelegramResults(ChosenInlineResultCommands.made_a_bet, offset);
            await MessageController.AnswerInlineQuery(User, InlineQuery.Id, results, newOffset);
        }

        public override bool Match()
        {
            if (InlineQuery.ChatType is not (ChatType.Group or ChatType.Supergroup)) return false;
            return InlineQuery.Query.StartsWith(CommandText);
        }
        
        public ShowBetStickers(User user, BotDatabaseContext context, InlineQuery inlineQuery) : base(user, context, inlineQuery)
        {
        }
    }
}