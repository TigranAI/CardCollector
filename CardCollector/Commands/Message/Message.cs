using System;
using System.Threading.Tasks;
using CardCollector.Controllers;
using System.Collections.Generic;
using System.Linq;
using CardCollector.Commands.Message.DocumentMessage;
using CardCollector.Commands.Message.TextMessage;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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
        private static readonly List<Message>
            TextCommandsList = new() {
                // Команда "Профиль"
                new ProfileMessage(),
                // Команда "/start"
                new StartMessage(),
                // Команда "Коллекция"
                new CollectionMessage(),
                // Команда "Магазин"
                new ShopMessage(),
                // Команда "Аукцион"
                new AuctionMessage(),
                // Ожидание ввода эмоджи
                new EnterEmojiMessage(),
                // Загрузка стикерпака
                new DownloadStickerPackMessage(),

                // Команда "Показать пример"
                new ShowSampleMessage()
            },
            FileCommandsList = new() {
                /* Выгрузка файлов к боту */
                new UploadFileMessage(),
            };

        /* Метод, создающий объекты команд исходя из полученного обновления */
        public static async Task<UpdateModel> Factory(Update update)
        {
            // Если сообщение от нашего бота
            if (update.Message!.From!.Username == AppSettings.NAME)
            {
                await Bot.Client.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId);
                return new IgnoreUpdate();
            }

            /* Список команд определяем исходя из типа сообщения */
            var list = update.Message.Type switch
            {
                MessageType.Text => TextCommandsList,
                MessageType.Document => FileCommandsList,
                _ => new List<Message>()
            };
            
            /* Данные определяем исходя из типа сообщения */
            var data = update.Message.Type switch
            {
                MessageType.Text => update.Message.Text,
                MessageType.Document => update.Message.Document.FileId,
                _ => ""
            };
            
            // Объект пользователя
            var user = await UserDao.GetUser(update.Message!.From);
            
            // Если пользователь заблокирован или пользователь - бот, то мы игнорируем дальнейшие действия
            if (user.IsBlocked || update.Message!.From!.IsBot) return new IgnoreUpdate();
        
            // Удаляем сообщение пользователя, оно нам больше не нужно
            await MessageController.DeleteMessage(user, update.Message.MessageId);
            
            // Возвращаем объект, если команда совпала
            foreach (var item in list.Where(item => item.IsMatches(data)))
                if(Activator.CreateInstance(item.GetType(), user, update) is Message executor)
                    if (executor.IsMatches(data)) return executor;
        
            // Возвращаем команда не найдена, если код дошел до сюда
            return new CommandNotFound(user, update, data);
        }

        protected Message(UserEntity user, Update update) : base(user, update) { }
        protected Message() { }
    }
}