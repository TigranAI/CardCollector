using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Commands.CallbackQueryHandler;
using CardCollector.Database.EntityDao;
using CardCollector.Extensions.Database.Entity;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Commands.MessageHandler.Profile;

[MenuPoint]
public class Profile : MessageHandler
{
    protected override string CommandText => MessageCommands.profile;
    protected override bool ClearMenu => true;
    protected override bool ClearStickers => true;

    protected override async Task Execute()
    {
        await User.Messages.ClearChat();
        User.Session.ResetModules();
        if (User.OpenStartPack == 0)
        {
            var income = User.Cash.GetIncome(User.Stickers);
            var currentLevel = await Context.Levels.FindLevel(User.Level.Level + 1);
            var expGoal = currentLevel?.LevelExpGoal.ToString() ?? "∞";
            var packsCount = User.Packs.Sum(item => item.Count);

            await User.Messages.SendMessage(User.GetProfileMessage(expGoal),
                Keyboard.GetProfileKeyboard(packsCount, User.InviteInfo, income));
        }
        else
        {
            var packInfo = await Context.Packs.FindById(1);
            await User.Messages.SendSticker(packInfo.PreviewFileId, OpenStartPacks());
            if (!User.FirstReward)
            {
                User.FirstReward = true;
                User.AddPack(packInfo, 7);
                await User.Messages.SendSticker(packInfo.PreviewFileId!, OpenStartPacks());
            }
        }
    }

    private InlineKeyboardMarkup OpenStartPacks()
    {
        return new InlineKeyboardMarkup(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.open_start_packs, CallbackQueryCommands.open_pack)}
        });
    }
}