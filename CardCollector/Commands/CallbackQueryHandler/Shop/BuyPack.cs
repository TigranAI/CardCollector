using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Commands.CallbackQueryHandler.Shop
{
    [MenuPoint]
    public class BuyPack : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.buy_pack;
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            await User.Messages.EditMessage(User, Messages.choose_option, Keyboard);
        }
        
        private InlineKeyboardMarkup Keyboard = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(Text.buy_random, $"{CallbackQueryCommands.select_shop_pack}=1")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(Text.buy_exclusive, 
                    $"{CallbackQueryCommands.choose_pack}={CallbackQueryCommands.select_shop_pack}=0=1")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(Text.buy_author,
                    $"{CallbackQueryCommands.choose_pack}={CallbackQueryCommands.select_shop_pack}=0=0")
            },
            new[] {InlineKeyboardButton.WithCallbackData(Text.info, CallbackQueryCommands.pack_info)},
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)},
        });
    }
}