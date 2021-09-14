using System;
using System.Threading.Tasks;
using CardCollector.Controllers;
using System.Collections.Generic;
using System.Linq;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message
{
    /* Родительский класс для входящих обновлений типа Message (обычное сообщение)
     при наследовании укажите ключевое слово, содержащееся в тексте
     для поля Command и определите логику действий в Execute
     Также необходимо определить констуктор с параметрами UserEntity и
     Update, наслеуемый от base(user, update)
     И После реализации добавить команду в список List в этом классе
     Для обработки команды определены следующие поля
     User - пользователь, вызвавший команду
     Update - обновление, полученное от сервера Телеграм */
    public abstract class Message : UpdateModel
    {
        /* Список команд */
        private static readonly List<Message> List = new()
        {
            // Команда "Профиль"
            new ProfileMessage(),
            // Команда "/start"
            new StartMessage()
        };

        /* Метод, создающий объекты команд исходя из полученного обновления */
        public static async Task<UpdateModel> Factory(Update update)
        {
            // Объект пользователя
            var user = await UserDao.GetUser(update.Message!.From);
            
            //Если сообщение не содержит текст
            if (update.Message!.Text == null) return new IgnoreUpdate(user, update);
            
            // Текст команды
            var command = update.Message!.Text;
        
            // Добавляем сообщения пользователя в пул для удаления
            await MessageController.DeleteMessage(user, update.Message.MessageId);
            
            // Возвращаем объект, если команда совпала
            foreach (var item in List.Where(item => item.IsMatches(command)))
                if(Activator.CreateInstance(item.GetType(), user, update) is Message executor)
                    if (executor.IsMatches(command)) return executor;
        
            // Возвращаем команда не найдена, если код дошел до сюда
            return new CommandNotFound(user, update, command);
        }

        protected Message(UserEntity user, Update update) : base(user, update) { }
        protected Message() { }
    }
}