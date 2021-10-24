using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.InlineQuery
{
    public class ShowCollectionStickers : InlineQueryCommand
    {
        protected override string CommandText => "";
        protected override bool ClearMenu => false;
        protected override bool AddToStack => false;

        public override async Task Execute()
        {
            // Получаем список стикеров
            var stickersList = await User.GetStickersList(Query);
            var results = User.Session.GetModule<FiltersModule>()
                .ApplyTo(stickersList).ToTelegramResults(Command.select_sticker);
            // Посылаем пользователю ответ на его запрос
            await MessageController.AnswerInlineQuery(InlineQueryId, results);
        }
        
        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            return update.InlineQuery?.ChatType is ChatType.Sender && user.Session.State == UserState.CollectionMenu;
        }

        public ShowCollectionStickers() { }
        public ShowCollectionStickers(UserEntity user, Update update) : base(user, update) { }
    }
}