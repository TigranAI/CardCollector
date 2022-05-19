using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.Resources;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.CallbackQueryHandler.Profile
{
    public class OpenPack : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.open_pack;
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            var packId = int.Parse(CallbackQuery.Data!.Split("=")[1]);
            var userPack = User.Packs.SingleOrDefault(item => item.Pack.Id == packId);
            if (userPack == null || userPack.Count < 1)
                await AnswerCallbackQuery(User, CallbackQuery.Id, Messages.packs_count_zero, true);
            else
            {
                var sticker = await userPack.Open();
                await User.Messages.ClearChat(User);
                await User.Messages.SendSticker(User, sticker.FileId);
                await User.Messages.SendMessage(User, $"{Messages.congratulation}\n{sticker}",
                    userPack.Count > 0
                        ? Keyboard.RepeatCommand(Text.open_more, CallbackQuery.Data!)
                        : Keyboard.BackKeyboard);
                await User.AddSticker(sticker, 1);
                
                if (User.InviteInfo?.TasksProgress is { } tp && !tp.OpenPack)
                {
                    tp.OpenPack = true;
                    await User.InviteInfo.CheckRewards(Context);
                }
            }
        }
    }
}