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

        public static InlineKeyboardMarkup GetSortingMenu(UserState state)
        {
            var keyboard = new List<InlineKeyboardButton[]>
            {
                new[] {InlineKeyboardButton.WithCallbackData(Text.author, $"{Command.author}=1")},
                new[] {InlineKeyboardButton.WithCallbackData(Text.tier, Command.tier)},
                new[] {InlineKeyboardButton.WithCallbackData(Text.emoji, Command.emoji)}
            };
            if (state != UserState.CollectionMenu) keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData(Text.price, Command.price)});
            keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData(Text.sort, Command.sort)});
            keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData(Text.cancel, Command.cancel)});
            keyboard.Add(new[] {InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(Text.show_stickers)});
            return new InlineKeyboardMarkup(keyboard);
        }
        /* Клавиатура меню сортировки */
        public static readonly InlineKeyboardMarkup SortOptions = new(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.no, $"{Command.set}={Command.sort}={SortingTypes.None}")},
            new[] {InlineKeyboardButton.WithCallbackData(SortingTypes.ByTierIncrease, $"{Command.set}={Command.sort}={SortingTypes.ByTierIncrease}")},
            new[] {InlineKeyboardButton.WithCallbackData(SortingTypes.ByTierDecrease, $"{Command.set}={Command.sort}={SortingTypes.ByTierDecrease}")},
            new[] {InlineKeyboardButton.WithCallbackData(SortingTypes.ByAuthor, $"{Command.set}={Command.sort}={SortingTypes.ByAuthor}")},
            new[] {InlineKeyboardButton.WithCallbackData(SortingTypes.ByTitle, $"{Command.set}={Command.sort}={SortingTypes.ByTitle}")},
            new[] {InlineKeyboardButton.WithCallbackData(Text.cancel, Command.back)},
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

        /* Клавиатура меню ввода эмоджи */
        public static readonly InlineKeyboardMarkup EmojiOptions = new (new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.all, $"{Command.set}={Command.emoji}=")},
            new[] {InlineKeyboardButton.WithCallbackData(Text.cancel, Command.back)},
        });

        /* Клавиатура меню выбора цен */
        public static readonly InlineKeyboardMarkup PriceOptions = new (new[]
        {
            new[] {
                InlineKeyboardButton.WithCallbackData($"{Text.from} 0", $"{Command.set}={Command.price}=0"),
                InlineKeyboardButton.WithCallbackData($"{Text.to} 100", $"{Command.set}={Command.price_to}=100"),
            },
            new[] {
                InlineKeyboardButton.WithCallbackData($"{Text.from} 100", $"{Command.set}={Command.price}=100"),
                InlineKeyboardButton.WithCallbackData($"{Text.to} 500", $"{Command.set}={Command.price_to}=500"),
            },
            new[] {
                InlineKeyboardButton.WithCallbackData($"{Text.from} 500", $"{Command.set}={Command.price}=500"),
                InlineKeyboardButton.WithCallbackData($"{Text.to} 1000", $"{Command.set}={Command.price_to}=1000"),
            },
            new[] {
                InlineKeyboardButton.WithCallbackData($"{Text.from} 1000", $"{Command.set}={Command.price}=1000"),
                InlineKeyboardButton.WithCallbackData($"{Text.to} ∞", $"{Command.set}={Command.price_to}=0"),
            },
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
                            InlineKeyboardButton.WithCallbackData(Text.previous, $"{Command.author}={page - 1}")
                        },
                        >=10 when page == 1 => new[]
                        {
                            InlineKeyboardButton.WithCallbackData(Text.next, $"{Command.author}={page + 1}")
                        },
                        _ => new[]
                        {
                            InlineKeyboardButton.WithCallbackData(Text.previous, $"{Command.author}={page - 1}"),
                            InlineKeyboardButton.WithCallbackData(Text.next, $"{Command.author}={page + 1}")
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