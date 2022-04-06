using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.MessageHandler.TextCommands
{
    public class Start : MessageHandler
    {
        protected override string CommandText => MessageCommands.start;
        
        protected override async Task Execute()
        {
            if (!User.FirstReward)
            {
                User.FirstReward = true;
                var packInfo = await Context.Packs.FindById(1);
                User.AddPack(packInfo, 7);
                await User.Messages.SendSticker(User, packInfo.PreviewFileId!);
                await User.Messages.SendMessage(User, Messages.first_reward, Keyboard.MyPacks, ParseMode.Html);
            }
            await User.Messages.SendMessage(User, Messages.start_message, Keyboard.Menu);
        }

        protected override async Task AfterExecute()
        {
            await MessageController.DeleteMessage(User.ChatId, Message.MessageId);
            await base.AfterExecute();
        }
    }
}