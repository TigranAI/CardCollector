using System.Linq;
using System.Threading.Tasks;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.ChosenInlineResultHandler.Collection
{
    public class SelectCombineSticker : ChosenInlineResultHandler
    {
        protected override string CommandText => ChosenInlineResultCommands.select_sticker;
        protected override async Task Execute()
        {
            var stickerId = long.Parse(ChosenInlineResult.ResultId.Split('=')[1]);
            var module = User.Session.GetModule<CombineModule>();
            module.Count = 1;
            module.SelectedStickerId = stickerId;
            var sticker = await Context.Stickers.FindById(stickerId);
            var userSticker = User.Stickers.Single(item => item.Sticker.Id == stickerId);
            await User.Messages.ClearChat(User);
            await User.Messages.SendSticker(User, sticker.FileId);
            await User.Messages.SendMessage(User, sticker.ToString(userSticker.Count), 
                Keyboard.GetCombineStickerKeyboard(module));
        }

        public override bool Match()
        {
            return base.Match() && User.Session.State == UserState.CombineMenu;
        }
    }
}