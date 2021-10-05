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
                // Команда "/menu"
                new MenuMessage(),
                // Команда "Коллекция"
                new CollectionMessage(),
                // Команда "Магазин"
                // TODO переписать магазин
                // new ShopMessage(),
                // Команда "Аукцион"
                new AuctionMessage(),
                // Ожидание ввода эмоджи
                new EnterEmojiMessage(),
                // Загрузка стикерпака
                new DownloadStickerPackMessage(),
                //команда ввода цены
                new EnterGemsPriceMessage(),
                // Команда "Показать пример"
                new ShowSampleMessage(),
                // Команда "Остановить"
                new StopBot()
            },
            FileCommandsList = new() {
                /* Выгрузка файлов к боту */
                new UploadFileMessage(),
            };

        /* Метод, создающий объекты команд исходя из полученного обновления */
        public static async Task<UpdateModel> Factory(Update update)
        {
            // Если сообщение от бота - игнорируем, нам не нужны боты
            if (update.Message!.From!.IsBot || update.Message.SuccessfulPayment is not null)
            {
                // Если это вдруг написал наш бот (сообщенияуведомления о закрпеах и пр.), то удаляем
                if (update.Message!.From!.Username == AppSettings.NAME)
                    await Bot.Client.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId);
                // Если это сообщение о платеже
                if (update.Message.SuccessfulPayment is not null)
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
                MessageType.Document => update.Message.Document!.FileId,
                _ => "Unknown"
            };
            
            // Объект пользователя
            var user = await UserDao.GetUser(update.Message!.From);
            
            // Если пользователь заблокирован или сообщение где-то в другом канале, привате - игонрируем
            if (user.IsBlocked || update.Message.Chat.Id != user.ChatId) return new IgnoreUpdate();
        
            // Удаляем сообщение пользователя в лс, оно нам больше не нужно
            await MessageController.DeleteMessage(user, update.Message.MessageId);
            
            // Если сообщение - это команда, полученная от бота, то мы игнорируем, так как получим ее через ChosenInlineResult
            if (update.Message.ViaBot is { }) return new IgnoreUpdate();
            
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