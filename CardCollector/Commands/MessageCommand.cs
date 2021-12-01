using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Commands.CallbackQuery;
using CardCollector.Commands.Message;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands
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
    public abstract class MessageCommand : UpdateModel
    {
        /* Список команд */
        private static readonly List<MessageCommand> List = new() {
            // Команда "Профиль"
            new Profile(),
            // Команда "/start"
            new Start(),
            // Команда "/menu"
            new Menu(),
            new Help(),
            // Команда "Коллекция"
            new Collection(),
            // Команда "Магазин"
            new Shop(),
            // Команда "Аукцион"
            new Auction(),
            // Ожидание ввода эмоджи
            new EnterEmoji(),
            new EnterGemsExchange(),
            //команда ввода цены
            new EnterGemsPrice(),
            new CreateToken(),
            /* Выгрузка файлов к боту */
            new UploadFile(),
            new UploadSticker(),
            new GiveExp(),
            new UploadForSaleSticker(),
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
            
            // Объект пользователя
            var user = await UserDao.GetUser(update.Message!.From);
            
            // Удаляем сообщение пользователя в лс, оно нам больше не нужно
            if (update.Message.Chat.Type is ChatType.Private)
                await MessageController.DeleteMessage(user, update.Message.MessageId);
            
            // Если пользователь заблокирован
            if (user.IsBlocked) return new IgnoreUpdate();
            
            // Возвращаем объект, если команда совпала
            /* Возвращаем первую подходящую команду */
            return List.FirstOrDefault(item => item.IsMatches(user, update)) is { } executor
                ? (UpdateModel) Activator.CreateInstance(executor.GetType(), user, update)
                : new IgnoreUpdate();
            /*CommandNotFound(user, update, update.Message.Type == MessageType.Text 
            ? update.Message.Text 
            : Utilities.ToJson(update.Message));*/
        }

        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            return update.Message?.Type == MessageType.Text && update.Message?.Text == CommandText;
        }

        protected MessageCommand() { }
        protected MessageCommand(UserEntity user, Update update) : base(user, update) { }
    }
}