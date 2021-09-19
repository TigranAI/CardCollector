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
            /*// Получаем MD5 хеш из полученного запроса разделением по символу '='
            var shortHash = InlineResult.Split('=')[1];
            // Вычитаем один стикер из общего их количества
            User.Stickers[shortHash].Count--;*/
            // Возвращаем CompletedTask, означающий завершение данного метода
            return Task.CompletedTask;
        }
        
        public SendStickerResult() { }
        public SendStickerResult(UserEntity user, Update update, string inlineResult)
            : base(user, update, inlineResult) { }
    }
}