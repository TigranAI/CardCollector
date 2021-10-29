using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.ChosenInlineResult
{
    public class StickerInfo : ChosenInlineResultCommand
    {
        protected override string CommandText => Command.sticker_info;
        
        public override async Task Execute()
        {
            var hash = InlineResult.Split('=')[1];
            var sticker = await StickerDao.GetByHash(hash);
            await MessageController.SendSticker(User, sticker.Id);
            await MessageController.SendMessage(User, sticker.ToString(), Keyboard.StickerInfoKeyboard);
        }

        public StickerInfo() { }
        public StickerInfo(UserEntity user, Update update) : base(user, update) { }
    }
}