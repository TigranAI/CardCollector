using System.Linq;
using System.Threading.Tasks;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.ChosenInlineResultHandler.Collection
{
    public class SelectCollectionSticker : ChosenInlineResultHandler
    {
        protected override string CommandText => ChosenInlineResultCommands.select_sticker;
        protected override async Task Execute()
        {
            var stickerId = long.Parse(ChosenInlineResult.ResultId.Split('=')[1]);
            var userSticker = User.Stickers.Single(item => item.Id == stickerId);
            if (User.Session.State is UserState.Default)
            {
                await User.Messages.ClearChat();
                await User.Messages.SendSticker(userSticker.Sticker.FileId);
                await User.Messages.SendMessage(userSticker.Sticker.ToString(userSticker.Count),
                    Keyboard.GetStickerKeyboard(userSticker.Sticker));
            }
            else
            {
                var module = User.Session.GetModule<CollectionModule>();
                module.Count = 1;
                module.SelectedStickerId = stickerId;
                await User.Messages.ClearChat();
                await User.Messages.SendSticker(userSticker.Sticker.FileId);
                await User.Messages.SendMessage(userSticker.Sticker.ToString(userSticker.Count),
                    Keyboard.GetCollectionStickerKeyboard(userSticker.Sticker, module.Count));
            }
        }

        public override bool Match()
        {
            return base.Match() && User.Session.State is UserState.CollectionMenu or UserState.Default;
        }
    }
}