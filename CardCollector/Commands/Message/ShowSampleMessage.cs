using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Commands.Message
{
    /* Этот класс можно использовать для тестирования или наброски эскизов
     Команда "Показать пример" доступна только пользователям с уровнем доступа "Разработчик" и выше
     PrivilegeLevel = 5 */
    public class ShowSampleMessage : Message
    {
        protected override string Command => MessageCommands.show_sample;
        public override async Task Execute()
        {
            await MessageController.SendMessage(User, "Текущие примененные фильтры:" +
                                                      "\nАвтор (все)\nТир (все)\nЭмоция (все)\nСортировка(нет)" +
                                                      "\n\nУстановите фильтры кнопками ниже:",
                new InlineKeyboardMarkup(new []
                {
                    new [] {InlineKeyboardButton.WithCallbackData("Автор")},
                    new [] {InlineKeyboardButton.WithCallbackData("Тир")},
                    new [] {InlineKeyboardButton.WithCallbackData("Эмоция")},
                    new [] {InlineKeyboardButton.WithCallbackData("Сортировка")},
                    new [] {InlineKeyboardButton.WithCallbackData("Отмена")},
                    new [] {InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("Показать стикеры")},
                }));
            await MessageController.SendMessage(User, "Выберите автора из списка ниже:",
                new InlineKeyboardMarkup(new []
                {
                    new [] {InlineKeyboardButton.WithCallbackData("Все")},
                    new [] {InlineKeyboardButton.WithCallbackData("А"),InlineKeyboardButton.WithCallbackData("Б")},
                    new [] {InlineKeyboardButton.WithCallbackData("В"),InlineKeyboardButton.WithCallbackData("Г"),},
                    new [] {InlineKeyboardButton.WithCallbackData("Д"),InlineKeyboardButton.WithCallbackData("Е"),},
                    new [] {InlineKeyboardButton.WithCallbackData("Ё"),InlineKeyboardButton.WithCallbackData("Ж"),},
                    new [] {InlineKeyboardButton.WithCallbackData("З"),InlineKeyboardButton.WithCallbackData("З"),},
                    new [] {InlineKeyboardButton.WithCallbackData("←"),InlineKeyboardButton.WithCallbackData("→")},
                    new [] {InlineKeyboardButton.WithCallbackData("Отмена")},
                }));
            await MessageController.SendMessage(User, "Текущие примененные фильтры:" +
                                                      "\nАвтор (Г)\nТир (все)\nЭмоция (все)\nСортировка(нет)" +
                                                      "\n\nУстановите фильтры кнопками ниже:",
                new InlineKeyboardMarkup(new []
                {
                    new [] {InlineKeyboardButton.WithCallbackData("Автор")},
                    new [] {InlineKeyboardButton.WithCallbackData("Тир")},
                    new [] {InlineKeyboardButton.WithCallbackData("Эмоция")},
                    new [] {InlineKeyboardButton.WithCallbackData("Сортировка")},
                    new [] {InlineKeyboardButton.WithCallbackData("Отмена")},
                    new [] {InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("Показать стикеры")},
                }));
        }

        /* Нужно помимо совпадения текста проверить пользователя на уровень привилегий */
        protected internal override bool IsMatches(string command)
        {
            return base.IsMatches(command) && User is not {PrivilegeLevel: < Constants.PROGRAMMER_PRIVILEGE_LEVEL};
        }
        
        public ShowSampleMessage(UserEntity user, Update update) : base(user, update) { }
        public ShowSampleMessage() { }
    }
}