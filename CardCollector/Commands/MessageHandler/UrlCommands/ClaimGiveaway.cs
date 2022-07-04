using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Commands.CallbackQueryHandler;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Commands.MessageHandler.UrlCommands;

[MenuPoint]
public class ClaimGiveaway : MessageUrlHandler
{
    protected override string CommandText => MessageUrlCommands.claim_giveaway;

    protected override async Task Execute()
    {
        await User.Messages.ClearChat();
        if (User.OpenStartPack == 0)
        {
            var giveaway = await Context.ChannelGiveaways.FindById(int.Parse(StartData[1]));
            if (giveaway == null || giveaway.IsEnded())
                await User.Messages.EditMessage(Messages.giveaway_now_ended, Keyboard.BackKeyboard);
            else if (giveaway.IsAwarded(User.Id))
                await User.Messages.EditMessage(Messages.you_are_now_awarded, Keyboard.BackKeyboard);
            else
            {
                await giveaway.Claim(User, Context);
                await User.Messages.EditMessage(
                    string.Format(Messages.you_got_from_this_giveaway, giveaway.PrizeText()), Keyboard.BackKeyboard);
            }
        }
        else
        {
            var packInfo = await Context.Packs.FindById(1);
            await User.Messages.SendSticker(packInfo.PreviewFileId, OpenStartPacks());
            if (!User.FirstReward)
            {
                User.FirstReward = true;
                User.AddPack(packInfo, 7);
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