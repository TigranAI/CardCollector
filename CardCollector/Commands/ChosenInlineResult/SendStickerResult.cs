using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Telegram.Bot.Types;

namespace CardCollector.Commands.ChosenInlineResult
{
    public class SendStickerResult : ChosenInlineResult
    {
        protected override string Command => "send_sticker";
        public override Task Execute()
        {
            var shortHash = InlineResult.Split('=')[1];
            User.Stickers[shortHash].Count--;
            return Task.CompletedTask;
        }
        
        public SendStickerResult(UserEntity user, Update update, string inlineResult)
            : base(user, update, inlineResult) { }

        public SendStickerResult() { }
    }
}