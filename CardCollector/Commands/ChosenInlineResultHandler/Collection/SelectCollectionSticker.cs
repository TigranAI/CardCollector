using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.ChosenInlineResultHandler.Collection
{
    public class SelectCollectionSticker : ChosenInlineResultHandler
    {
        protected override string CommandText => ChosenInlineResultCommands.select_sticker;
        protected override async Task Execute()
        {
            var stickerId = long.Parse(ChosenInlineResult.ResultId.Split('=')[1]);
            var sticker = await Context.Stickers.FindById(stickerId);
            var userSticker = User.Stickers.Single(item => item.Sticker.Id == stickerId);
            if (User.Session.State is UserState.Default)
            {
                await User.Messages.SendSticker(User, sticker.FileId);
                await User.Messages.EditMessage(User, sticker.ToString(userSticker.Count),
                    Keyboard.GetStickerKeyboard(sticker));
            }
            else
            {
                var module = User.Session.GetModule<CollectionModule>();
                module.SelectedStickerId = stickerId;
                await User.Messages.SendSticker(User, sticker.FileId);
                await User.Messages.EditMessage(User, sticker.ToString(userSticker.Count),
                    Keyboard.GetCollectionStickerKeyboard(sticker, module.Count));
            }
        }

        public override bool Match()
        {
            return base.Match() && User.Session.State is UserState.CollectionMenu or UserState.Default;
        }

        public SelectCollectionSticker(User user, BotDatabaseContext context, ChosenInlineResult chosenInlineResult) : base(user, context, chosenInlineResult)
        {
        }
    }
}