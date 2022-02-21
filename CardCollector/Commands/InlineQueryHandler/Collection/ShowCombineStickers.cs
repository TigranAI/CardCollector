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
    public class ShowCombineStickers : InlineQueryHandler
    {
        protected override async Task Execute()
        {
            var module = User.Session.GetModule<CombineModule>();
            var sticker = module.CombineList.FirstOrDefault()?.Item1;
            var stickersList = User.Stickers
                .Where(item => item.Count > 0)
                .Select(item => item.Sticker)
                .Where(item => item.Contains(InlineQuery.Query) 
                                      && (sticker == null || sticker.Tier == item.Tier))
                .ToList();
            stickersList.RemoveAll(item =>
                module.CombineList.Any(pair => pair.Item1.Id == item.Id));
            
            var offset = int.Parse(InlineQuery.Offset == "" ? "0" : InlineQuery.Offset);
            var newOffset = offset + 50 > stickersList.Count ? "" : (offset + 50).ToString();
            
            var results = stickersList.ToTelegramStickersAsMessage(ChosenInlineResultCommands.select_sticker, offset);
            await MessageController.AnswerInlineQuery(User, InlineQuery.Id, results, newOffset);
        }
        
        public override bool Match()
        {
            return User.Session.State == UserState.CombineMenu && InlineQuery.ChatType is ChatType.Sender;
        }

        public ShowCombineStickers(User user, BotDatabaseContext context, InlineQuery inlineQuery) : base(user, context, inlineQuery)
        {
        }
    }
}