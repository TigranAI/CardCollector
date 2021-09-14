using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    /* Родительский класс для входящих обновлений типа CallbackQuery (нажатие пользователем инлайн кнопки)
     при наследовании укажите ключевое слово, содержащееся в запросе
     для поля Command и определите логику действий в Execute
     Также необходимо определить констуктор с параметрами UserEntity
     и Update, наслеуемый от base(user, update)
     И После реализации добавить команду в список List в этом классе
     Для обработки команды определены следующие поля
     User - пользователь, вызвавший команду
     Update - обновление, полученное от сервера Телеграм */
    public abstract class CallbackQuery : UpdateModel
    {
        /* Список команд, распознаваемых ботом */
        private static readonly List<CallbackQuery> List = new()
        {

        };

        /* Метод, создающий объекты команд исходя из полученного обновления */
        public static async Task<UpdateModel> Factory(Update update)
        {
            // Текст команды
            var command = update.CallbackQuery!.Data;

            // Объект пользователя
            var user = await UserDao.GetUser(update.CallbackQuery.From);

            // Возвращаем объект, если команда совпала
            foreach (var item in List.Where(item => item.IsMatches(command)))
                if (Activator.CreateInstance(item.GetType(), user, update) is CallbackQuery executor && executor.IsMatches(command))
                    return executor;

            // Возвращаем команда не найдена, если код дошел до сюда
            return new CommandNotFound(user, update, command);
        }

        protected CallbackQuery(UserEntity user, Update update) : base(user, update) { }
        protected CallbackQuery() { }
    }
}