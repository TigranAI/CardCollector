using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes.Menu;
using CardCollector.Commands.CallbackQueryHandler;
using CardCollector.Controllers;
using CardCollector.Database;
using CardCollector.Others;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.InlineQueryHandler.Channel
{
    [DontAddToCommandStack]
    public class ShowStickers : InlineQueryHandler
    {
        protected override async Task Execute()
        {
            var stickersList = User.Stickers
                .Where(item => item.Count > 0)
                .Select(item => item.Sticker)
                .Where(item => item.Contains(InlineQuery.Query))
                .ToList();
            var offset = int.Parse(InlineQuery.Offset == "" ? "0" : InlineQuery.Offset);
            var newOffset = offset + 50 > stickersList.Count() ? "" : (offset + 50).ToString();
            
            var results = stickersList
                .ToTelegramStickers(CallbackQueryCommands.ignore, offset);
            await MessageController.AnswerInlineQuery(User, InlineQuery.Id, results, newOffset);
        }

        public override bool Match()
        {
            return InlineQuery.ChatType is ChatType.Channel;
        }
        
        public ShowStickers(User user, BotDatabaseContext context, InlineQuery inlineQuery) : base(user, context, inlineQuery)
        {
        }
    }
}