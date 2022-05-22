using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Commands.CallbackQueryHandler.Profile;

[MenuPoint]
public class MyPacks : CallbackQueryHandler
{
    protected override string CommandText => CallbackQueryCommands.my_packs;
    protected override bool ClearStickers => true;

    protected override async Task Execute()
    {
        var randomCount = User.Packs.Sum(item => item.Pack.Id == 1 ? item.Count : 0);
        var authorCount = User.Packs.Sum(item => item.Pack.Id != 1 && !item.Pack.IsExclusive ? item.Count : 0);
        var exclusiveCount = User.Packs.Sum(item => item.Pack.Id != 1 && item.Pack.IsExclusive ? item.Count : 0);
        await User.Messages.EditMessage(
            $"{Messages.your_packs}" +
            $"\n{Messages.random_packs}: {randomCount}{Text.items}" +
            $"\n{Messages.author_pack}: {authorCount}{Text.items}" +
            $"\n{Messages.exlusive_pack}: {exclusiveCount}{Text.items}",
            Keyboard());
    }

    private InlineKeyboardMarkup Keyboard()
    {
        return new InlineKeyboardMarkup(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.open_random,
                $"{CallbackQueryCommands.open_pack}")},
            new[]
            {
                InlineKeyboardButton.WithCallbackData(Text.open_author,
                    $"{CallbackQueryCommands.choose_user_pack}={CallbackQueryCommands.open_pack}=0=0")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(Text.open_exclusive,
                    $"{CallbackQueryCommands.choose_user_pack}={CallbackQueryCommands.open_pack}=0=1")
            },
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)},
        });
    }
}