using System.Threading.Tasks;
using CardCollector.Commands.Message;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class SelectEmoji : CallbackQueryCommand
    {
        protected override string CommandText => Command.emoji;
        public override async Task Execute()
        {
            EnterEmoji.AddToQueue(User.Id, CallbackMessageId);
            await MessageController.EditMessage(User, CallbackMessageId, Messages.enter_emoji, Keyboard.EmojiOptions);
        }

        public SelectEmoji() { }
        public SelectEmoji(UserEntity user, Update update) : base(user, update) { }
    }
}