using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.Message
{
    /* Обработка команды "/start" */
    public class Start : MessageHandler
    {
        protected override string CommandText => Text.start;
        private const string PackStickerId = "CAACAgIAAxkBAAIWs2DuY4vB50ARmyRwsgABs_7o5weDaAAC-g4AAmq4cUtH6M1FoN4bxSAE";
        
        public override async Task Execute()
        {
            await User.ClearChat();
            if (!User.FirstReward)
            {
                User.FirstReward = true;
                var randomPack = await UserPacksDao.GetOne(User.Id, 1);
                randomPack.Count += 7;
                await MessageController.SendSticker(User, PackStickerId);
                await MessageController.SendMessage(User, Messages.first_reward, Keyboard.MyPacks, ParseMode.Html);
            }
            /* Отправляем пользователю сообщение со стандартной клавиатурой */
            await MessageController.SendMessage(User, Messages.start_message, Keyboard.Menu);
        }
        
        public Start(UserEntity user, Update update) : base(user, update) { }
    }
}