using System.Threading.Tasks;
using CardCollector.Commands.Message;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class EmojiCallback : CallbackQuery
    {
        protected override string CommandText => Command.emoji;
        public override async Task Execute()
        {
            EnterEmojiMessage.AddToQueue(User.Id, CallbackMessageId);
            await MessageController.EditMessage(User, CallbackMessageId, Messages.enter_emoji, Keyboard.EmojiOptions);
        }

        public EmojiCallback() { }
        public EmojiCallback(UserEntity user, Update update) : base(user, update) { }
    }
}