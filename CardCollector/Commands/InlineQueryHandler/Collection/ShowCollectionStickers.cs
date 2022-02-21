using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes.Menu;
using CardCollector.Commands.ChosenInlineResultHandler;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.InlineQueryHandler.Collection
{
    [DontAddToCommandStack]
    public class ShowCollectionStickers : InlineQueryHandler
    {
        protected override async Task Execute()
        {
            var filters = User.Session.GetModule<FiltersModule>();
            var stickersList = User.Stickers
                .Where(item => item.Count > 0)
                .Select(item => item.Sticker)
                .ToList();
            stickersList = filters.ApplyTo(stickersList);
            stickersList.RemoveAll(item => !item.Contains(InlineQuery.Query));
            var offset = int.Parse(InlineQuery.Offset == "" ? "0" : InlineQuery.Offset);
            var newOffset = offset + 50 > stickersList.Count ? "" : (offset + 50).ToString();
            var results = stickersList.ToTelegramStickersAsMessage(ChosenInlineResultCommands.select_sticker, offset);
            await MessageController.AnswerInlineQuery(User, InlineQuery.Id, results, newOffset);
        }

        public override bool Match()
        {
            if (User.Session.State is not (UserState.CollectionMenu or UserState.Default)) return false;
            return InlineQuery.ChatType is ChatType.Sender;
        }

        public ShowCollectionStickers(User user, BotDatabaseContext context, InlineQuery inlineQuery) : base(user,
            context, inlineQuery)
        {
        }
    }
}