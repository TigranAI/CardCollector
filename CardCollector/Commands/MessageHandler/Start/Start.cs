using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.MessageHandler.Start
{
    public class Start : MessageHandler
    {
        protected override string CommandText => Text.start;
        private const string PACK_STICKER_ID = "CAACAgIAAxkBAAIWs2DuY4vB50ARmyRwsgABs_7o5weDaAAC-g4AAmq4cUtH6M1FoN4bxSAE";
        
        protected override async Task Execute()
        {
            if (!User.FirstReward)
            {
                User.FirstReward = true;
                var randomPack = User.Packs.SingleOrDefault(pack => pack.Id == 1);
                if (randomPack != null) randomPack.Count += 7;
                else
                {
                    var packInfo = await Context.Packs.FindPack(1);
                    User.Packs.Add(new UserPacks(User, packInfo, 7));
                }
                await MessageController.SendSticker(User, PACK_STICKER_ID);
                await MessageController.SendMessage(User, Messages.first_reward, Keyboard.MyPacks, ParseMode.Html);
            }
            await MessageController.SendMessage(User, Messages.start_message, Keyboard.Menu);
        }

        public override bool Match()
        {
            return Message.Type == MessageType.Text && Message.Text!.Split("@")[0] == CommandText;
        }

        public Start(User user, BotDatabaseContext context, Message message) : base(user, context, message)
        {
        }
    }
}