using System.Threading.Tasks;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.CallbackQueryHandler.Admin;

public class AddPuzzle : CallbackQueryHandler
{
    protected override string CommandText => CallbackQueryCommands.add_puzzle;
    protected override async Task Execute()
    {
        var packId = int.Parse(CallbackQuery.Data!.Split("=")[1]);
        User.Session.GetModule<AdminModule>().SelectedPackId = packId;
        await User.Messages.EditMessage(Messages.choose_sticker, Keyboard.ShowStickers);
        User.Session.State = UserState.AddPuzzle;
    }
}