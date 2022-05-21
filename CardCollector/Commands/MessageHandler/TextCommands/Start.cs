using System.Linq;
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
                await User.Messages.SendSticker(packInfo.PreviewFileId!);
                await User.Messages.SendMessage(Messages.first_reward, Keyboard.MyPacks, ParseMode.Html);
            }
            
            var isFirstOrderPicked = User.SpecialOrdersUser.Any(item => item.Id == 2);
            await User.Messages.SendMessage(Messages.start_message, Keyboard.Menu(isFirstOrderPicked));
        }

        protected override async Task AfterExecute()
        {
            await DeleteMessage(User.ChatId, Message.MessageId);
            await base.AfterExecute();
        }
    }
}