using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Database.EntityDao;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.CallbackQueryHandler.Shop
{
    [MenuPoint]
    public class SelectShopPack : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.select_shop_pack;
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            var packId = int.Parse(CallbackQuery.Data!.Split('=')[1]);
            var packInfo = await Context.Packs.FindById(packId);
            User.Session.GetModule<ShopModule>().SelectedPackId = packId;
            if (packInfo.PreviewFileId == null)
            {
                var sticker = packInfo.Stickers.Random();
                await User.Messages.SendSticker(User, sticker.ForSaleFileId ?? sticker.FileId,
                    Keyboard.ShopPackKeyboard(packInfo));
            }
            else
                await User.Messages.SendSticker(User, packInfo.PreviewFileId,
                    Keyboard.ShopPackKeyboard(packInfo));
        }
    }
}