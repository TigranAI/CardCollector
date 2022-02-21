using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.MessageHandler.TextCommands
{
    public class Start : MessageHandler
    {
        protected override string CommandText => MessageCommands.start;
        private const string PACK_STICKER_ID = "CAACAgIAAxkBAAIWs2DuY4vB50ARmyRwsgABs_7o5weDaAAC-g4AAmq4cUtH6M1FoN4bxSAE";
        
        protected override async Task Execute()
        {
            if (!User.FirstReward)
            {
                User.FirstReward = true;
                var packInfo = await Context.Packs.FindById(1);
                User.AddPack(packInfo, 7);
                await User.Messages.SendSticker(User, PACK_STICKER_ID);
                await User.Messages.SendMessage(User, Messages.first_reward, Keyboard.MyPacks, ParseMode.Html);
            }
            await User.Messages.SendMessage(User, Messages.start_message, Keyboard.Menu);
        }

        protected override async Task AfterExecute()
        {
            await MessageController.DeleteMessage(User.ChatId, Message.MessageId);
            await base.AfterExecute();
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