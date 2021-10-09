using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.ChosenInlineResult
{
    /* Данная команда выполняется при отправке пользователем стикера */
    public class SendStickerResult : ChosenInlineResult
    {
        /* Ключевое слово для данной команды send_sticker */
        protected override string CommandText => Command.send_sticker;
        public override Task Execute()
        {
            return Task.CompletedTask;
        }
        
        public SendStickerResult() { }
        public SendStickerResult(UserEntity user, Update update) : base(user, update) { }
    }
}