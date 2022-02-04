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
    public class ShowCombineStickers : InlineQueryHandler
    {
        public override async Task Execute()
        {
            var module = User.Session.GetModule<CombineModule>();
            // Получаем список стикеров
            var stickersList = await User.GetStickersList(Query, module.Tier);
            stickersList = stickersList.Except(module.CombineList.Keys);
            var results = stickersList.ToTelegramResults(Command.select_sticker);
            // Посылаем пользователю ответ на его запрос
            await MessageController.AnswerInlineQuery(InlineQueryId, results);
        }
        
        protected internal override bool Match(UserEntity user, Update update)
        {
            return update.InlineQuery?.ChatType is ChatType.Sender && user.Session.State == UserState.CombineMenu;
        }

        public ShowCombineStickers(UserEntity user, Update update) : base(user, update) { }
    }
}