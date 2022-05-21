using System.Threading.Tasks;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.ChosenInlineResultHandler.Admin;

public class AddPuzzle : ChosenInlineResultHandler
{
    protected override string CommandText => ChosenInlineResultCommands.add_puzzle;
    protected override async Task Execute()
    {
        var stickerId = long.Parse(ChosenInlineResult.ResultId.Split("=")[1]);
        User.Session.GetModule<AdminModule>().SelectedStickerId = stickerId;
        var sticker = await Context.Stickers.FindById(stickerId);
        await User.Messages.ClearChat();
        await User.Messages.SendSticker(sticker.FileId);
        await User.Messages.SendMessage(Messages.send_puzzle_archive, Keyboard.BackKeyboard);
        User.Session.State = UserState.SendPuzzleArchive;
    }
}