using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes.Menu;
using CardCollector.Commands.ChosenInlineResultHandler;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.InlineQueryHandler.Admin
{
    [DontAddToCommandStack]
    public class ShowStickersToEdit : InlineQueryHandler
    {
        protected override async Task Execute()
        {
            var packId = User.Session.GetModule<AdminModule>().SelectedPackId;
            var pack = await Context.Packs.FindById(packId);
            var stickersList = pack.Stickers.Where(item => item.Contains(InlineQuery.Query)).ToList();
            stickersList.Sort(new TierComparer());
            var offset = int.Parse(InlineQuery.Offset == "" ? "0" : InlineQuery.Offset);
            var newOffset = offset + 50 > stickersList.Count ? "" : (offset + 50).ToString();
            var results = stickersList
                .ToTelegramStickersAsMessage(ChosenInlineResultCommands.select_edit_sticker, offset);
            await MessageController.AnswerInlineQuery(User, InlineQuery.Id, results, newOffset);
        }

        public override bool Match()
        {
            if (InlineQuery.ChatType is not ChatType.Sender) return false;
            if (User.PrivilegeLevel < PrivilegeLevel.Programmer) return false;
            if (User.Session.State is not (UserState.EditSticker or UserState.LoadForSaleSticker)) return false;
            return User.Session.GetModule<AdminModule>().SelectedPackId is not null;
        }

        public ShowStickersToEdit(User user, BotDatabaseContext context, InlineQuery inlineQuery) : base(user, context,
            inlineQuery)
        {
        }
    }
}