using System.Threading.Tasks;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.ChosenInlineResultHandler.Admin
{
    public class SelectEditSticker : ChosenInlineResultHandler
    {
        protected override string CommandText => ChosenInlineResultCommands.select_edit_sticker;
        protected override async Task Execute()
        {
            var stickerId = long.Parse(ChosenInlineResult.ResultId.Split('=')[1]);
            User.Session.GetModule<AdminModule>().SelectedStickerId = stickerId;
            var sticker = await Context.Stickers.FindById(stickerId);
            await User.Messages.ClearChat(User);
            await User.Messages.SendSticker(User, sticker.FileId);
            await User.Messages.SendMessage(User, Messages.upload_new_file, Keyboard.BackKeyboard);
        }
    }
}