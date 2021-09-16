using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Resources
{
    /* В данном классе содержатся все клавиатуры, используемые в проекте */
    public static class Keyboard
    {
        /* Клавиатура, отображаемая вместе с сообщением профиля */
        public static readonly InlineKeyboardMarkup ProfileKeyboard = new(new[]
            {
                InlineKeyboardButton.WithCallbackData(CallbackQueryCommands.collect_income)
            }
        );

        /* Клавиатура, отображаемая с первым сообщением пользователя */
        public static readonly ReplyKeyboardMarkup Menu = new(new[]
        {
            new KeyboardButton[] {MessageCommands.profile, MessageCommands.collection},
            new KeyboardButton[] {MessageCommands.shop, MessageCommands.auction},
        }) {ResizeKeyboard = true};

        /* Клавиатура меню сортировки */
        public static readonly InlineKeyboardMarkup SortingOptions = new(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(CallbackQueryCommands.author)},
            new[] {InlineKeyboardButton.WithCallbackData(CallbackQueryCommands.tier)},
            new[] {InlineKeyboardButton.WithCallbackData(CallbackQueryCommands.emoji)},
            new[] {InlineKeyboardButton.WithCallbackData(CallbackQueryCommands.sorting)},
            new[] {InlineKeyboardButton.WithCallbackData(CallbackQueryCommands.cancel)},
            new[] {InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(CallbackQueryCommands.show_stickers)},
        });


        /* Возвращает клавиатуру со списоком авторов */
        public static InlineKeyboardMarkup GetAuthorsKeyboard(List<string> list, int page = 1)
        {
            /* Список авторов, отображаемый на текущей странице */
            var sublist = list.GetRange((page - 1) * 10,
                list.Count >= page * 10 ? 10 : list.Count % 10);
            /* Список кнопок на клавиатуре */
            var keyboardList = new List<InlineKeyboardButton[]>
            {
                new[]
                {
                    /* Добавляем в список кнопку "Все" */
                    InlineKeyboardButton.WithCallbackData(CallbackQueryCommands.All,
                        $"{CallbackQueryCommands.author_callback}=")
                }
            };
            /* Копируем список */
            var copyList = sublist.ToList();
            while (copyList.Count > 0)
            {
                /* Берем первый элемент и запихиваем его в строку */
                var author = copyList[0];
                copyList.RemoveAt(0);
                var keyRow = new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(author,
                            $"{CallbackQueryCommands.author_callback}={author}")
                };
                /* Если есть еще элементы, то добавляем в строку вторую кнопку */
                if (copyList.Count > 0)
                {
                    author = copyList[0];
                    copyList.RemoveAt(0);
                    keyRow.Add(InlineKeyboardButton.WithCallbackData(author,
                            $"{CallbackQueryCommands.author_callback}={author}"));
                }
                /* Добавляем строку кнопок в клавиатуру */
                keyboardList.Add(keyRow.ToArray());
            }

            /* Если всего авторов больше 10, то добавляем стрелочки */
            if (list.Count > 10)
                keyboardList.Add(
                    sublist.Count switch
                    {
                        <10 => new[]
                        {
                            InlineKeyboardButton.WithCallbackData(CallbackQueryCommands.previous,
                                $"{CallbackQueryCommands.change_page}={page - 1}")
                        },
                        >=10 when page == 1 => new[]
                        {
                            InlineKeyboardButton.WithCallbackData(CallbackQueryCommands.next,
                                $"{CallbackQueryCommands.change_page}={page + 1}")
                        },
                        _ => new[]
                        {
                            InlineKeyboardButton.WithCallbackData(CallbackQueryCommands.previous,
                                $"{CallbackQueryCommands.change_page}={page - 1}"),
                            InlineKeyboardButton.WithCallbackData(CallbackQueryCommands.next,
                                $"{CallbackQueryCommands.change_page}={page + 1}")
                        }
                    }
                );
            /* Добавляем кнопку отмены */
            keyboardList.Add(new[]
            {
                InlineKeyboardButton.WithCallbackData(CallbackQueryCommands.cancel,
                    CallbackQueryCommands.back)
            });
            /* Вовзращаем клавиатуру */
            return new InlineKeyboardMarkup(keyboardList);
        }
    }
}