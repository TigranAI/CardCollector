using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using Telegram.Bot.Types;

namespace CardCollector.Commands.InlineQuery
{
    public class ShowStickersInGroup : InlineQuery
    {
        protected override string Command => "";
        
        public override async Task Execute()
        {
            var filter = Update.InlineQuery!.Query;
            var stickersList = await User.GetStickersList("send_sticker",filter);
            await MessageController.AnswerInlineQuery(InlineQueryId, stickersList, "название");
        }

        protected override bool IsMatches(string command)
        {
            return command.Contains("Group") || command.Contains("Supergroup") || command.Contains("Private");
        }

        public ShowStickersInGroup(UserEntity user, Update update, string inlineQueryId)
            : base(user, update, inlineQueryId) { }
        public ShowStickersInGroup() { }
    }
}