using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.InlineQuery
{
    public class ShowAuctionStickers : InlineQuery
    {
        protected override string CommandText => "";
        
        public override async Task Execute()
        {
            // Получаем список стикеров
            var stickersList = (await AuctionController.GetStickers(Query)).AsEnumerable();
            stickersList = User.Session.GetModule<FiltersModule>()
                .ApplyTo(stickersList);
            var results = User.Session.GetModule<FiltersModule>()
                .ApplyPriceTo(stickersList)
                .ToTelegramResults(Command.select_sticker);
            // Посылаем пользователю ответ на его запрос
            await MessageController.AnswerInlineQuery(InlineQueryId, results);
        }

        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            return update.InlineQuery?.ChatType is ChatType.Sender && user.Session.State == UserState.AuctionMenu;
        }

        public ShowAuctionStickers() { }
        public ShowAuctionStickers(UserEntity user, Update update) : base(user, update) { }
    }
}