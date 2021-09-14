using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Resources
{
    /* В данном классе содержатся все клавиатуры, используемые в проекте */
    public static class Keyboard
    {
        /* Клавиатура, отображаемая вместе с сообщением профиля */
        public static readonly InlineKeyboardMarkup ProfileKeyboard = new (new[]
            {
                InlineKeyboardButton.WithCallbackData(CallbackQueryCommands.collect_income)
            }
        );
        
        /* Клавиатура, отображаемая с первым сообщением пользователя */
        public static readonly ReplyKeyboardMarkup Menu = new (new []
        {
            new KeyboardButton[] { MessageCommands.profile, MessageCommands.collection },
            new KeyboardButton[] { MessageCommands.shop, MessageCommands.auction },
        }) { ResizeKeyboard = true };
    }
}