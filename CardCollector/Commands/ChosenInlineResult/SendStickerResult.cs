using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using Telegram.Bot.Types;

namespace CardCollector.Commands.ChosenInlineResult
{
    public class SendStickerResult : ChosenInlineResult
    {
        protected override string Command => "send_sticker";
        public override async Task<Telegram.Bot.Types.Message> Execute()
        {
            var shortHash = InlineResult.Split('=')[1];
            var stickerRelation = await UserStickerRelationDao.GetByShortHash(shortHash);
            stickerRelation.Count--;
            return new Telegram.Bot.Types.Message();
        }
        
        public SendStickerResult(UserEntity user, Update update, string inlineResult)
            : base(user, update, inlineResult) { }

        public SendStickerResult() { }
    }
}