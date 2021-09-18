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
                InlineKeyboardButton.WithCallbackData(Text.collect_income, Command.collect_income)
            }
        );

        /* Клавиатура, отображаемая с первым сообщением пользователя */
        public static readonly ReplyKeyboardMarkup Menu = new(new[]
        {
            new KeyboardButton[] {Text.profile, Text.collection},
            new KeyboardButton[] {Text.shop, Text.auction},
        }) {ResizeKeyboard = true};

        /* Клавиатура меню сортировки */
        public static readonly InlineKeyboardMarkup SortingOptions = new(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.author, Command.author)},
            new[] {InlineKeyboardButton.WithCallbackData(Text.tier, Command.tier)},
            new[] {InlineKeyboardButton.WithCallbackData(Text.emoji, Command.emoji)},
            new[] {InlineKeyboardButton.WithCallbackData(Text.sort, Command.sort)},
            new[] {InlineKeyboardButton.WithCallbackData(Text.cancel, Command.cancel)},
            new[] {InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(Text.show_stickers)},
        });

        /* Клавиатура меню выбора тира */
        public static readonly InlineKeyboardMarkup TierOptions = new (new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.all, $"{Command.set}={Command.tier}=-1")},
            new[] {InlineKeyboardButton.WithCallbackData("1", $"{Command.set}={Command.tier}=1")},
            new[] {InlineKeyboardButton.WithCallbackData("2", $"{Command.set}={Command.tier}=2")},
            new[] {InlineKeyboardButton.WithCallbackData("3", $"{Command.set}={Command.tier}=3")},
            new[] {InlineKeyboardButton.WithCallbackData("4", $"{Command.set}={Command.tier}=4")},
            new[] {InlineKeyboardButton.WithCallbackData("5", $"{Command.set}={Command.tier}=5")},
            new[] {InlineKeyboardButton.WithCallbackData(Text.cancel, Command.back)},
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
                    InlineKeyboardButton.WithCallbackData(Text.all, 
                        $"{Command.set}={Command.author}=")
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
                            $"{Command.set}={Command.author}={author}")
                };
                /* Если есть еще элементы, то добавляем в строку вторую кнопку */
                if (copyList.Count > 0)
                {
                    author = copyList[0];
                    copyList.RemoveAt(0);
                    keyRow.Add(InlineKeyboardButton.WithCallbackData(author,
                            $"{Command.set}={Command.author}={author}"));
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
                            InlineKeyboardButton.WithCallbackData(Text.previous, $"{Command.change_page}={page - 1}")
                        },
                        >=10 when page == 1 => new[]
                        {
                            InlineKeyboardButton.WithCallbackData(Text.next, $"{Command.change_page}={page + 1}")
                        },
                        _ => new[]
                        {
                            InlineKeyboardButton.WithCallbackData(Text.previous, $"{Command.change_page}={page - 1}"),
                            InlineKeyboardButton.WithCallbackData(Text.next, $"{Command.change_page}={page + 1}")
                        }
                    }
                );
            /* Добавляем кнопку отмены */
            keyboardList.Add(new[] {
                InlineKeyboardButton.WithCallbackData(Text.cancel, Command.back)
            });
            /* Вовзращаем клавиатуру */
            return new InlineKeyboardMarkup(keyboardList);
        }
    }
}