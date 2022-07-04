using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Commands.CallbackQueryHandler;
using CardCollector.Database.EntityDao;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Commands.MessageHandler.TextCommands;

[MenuPoint]
public class Start : MessageHandler
{
    protected override string CommandText => MessageCommands.start;
        
    protected override async Task Execute()
    {
        await User.Messages.ClearChat();
        var isFirstOrderPicked = User.SpecialOrdersUser.Any(item => item.Id == 2);
        await User.Messages.SendStartMessage(isFirstOrderPicked);
        var packInfo = await Context.Packs.FindById(1);
        if (!User.FirstReward)
        {
            User.FirstReward = true;
            User.AddPack(packInfo, 7);
        }
        if (User.OpenStartPack > 0)
            await User.Messages.SendSticker(packInfo.PreviewFileId!, OpenStartPacks());
    }

    private InlineKeyboardMarkup OpenStartPacks()
    {
        return new InlineKeyboardMarkup(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.open_start_packs, CallbackQueryCommands.open_pack)}
        });
    }

    protected override async Task AfterExecute()
    {
        await DeleteMessage(User.ChatId, Message.MessageId);
        await base.AfterExecute();
    }
}