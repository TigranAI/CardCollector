using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Commands.ChosenInlineResultHandler;
using CardCollector.Database.EntityDao;
using CardCollector.Extensions;
using CardCollector.Others;
using CardCollector.Resources.Enums;
using CardCollector.Session.Modules;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.InlineQueryHandler.Admin
{
    [SkipCommand]
    public class ShowStickersToEdit : InlineQueryHandler
    {
        protected override async Task Execute()
        {
            var offset = Offset.Of(InlineQuery);
            var length = 0;
            
            var packId = User.Session.GetModule<AdminModule>().SelectedPackId;
            var pack = await Context.Packs.FindById(packId);
            var results = pack.Stickers
                .Where(item => item.Contains(InlineQuery.Query))
                .OrderBy(sticker => sticker.Tier)
                .And(list => length = list.Count())
                .ToTelegramResults(ChosenInlineResultCommands.select_edit_sticker, offset);
            
            await AnswerInlineQuery(User, InlineQuery.Id, results, offset.GetNext(length));
        }

        public override bool Match()
        {
            if (InlineQuery.ChatType is not ChatType.Sender) return false;
            if (User.PrivilegeLevel < PrivilegeLevel.Programmer) return false;
            if (User.Session.State is not (UserState.EditSticker or UserState.LoadForSaleSticker)) return false;
            return User.Session.GetModule<AdminModule>().SelectedPackId is not null;
        }
    }
}