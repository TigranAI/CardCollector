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
    [Attributes.Message, Attributes.Abstract]
    public abstract class MessageHandler : HandlerModel
    {
        /* Список команд */
        public static List<Type> Commands = new();
        
        /* Метод, создающий объекты команд исходя из полученного обновления */
        public static async Task<HandlerModel> Factory(Update update)
        {
            // Если сообщение от бота - игнорируем, нам не нужны боты
            if (update.Message!.From!.IsBot)
            {
                // Если это вдруг написал наш бот (сообщенияуведомления о закрпеах и пр.), то удаляем
                if (update.Message!.From!.Username == AppSettings.NAME)
                    await Bot.Client.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId);
                return new IgnoreHandler();
            }
            
            // Объект пользователя
            var user = await UserDao.GetUser(update.Message!.From);
            
            // Удаляем сообщение пользователя в лс, оно нам больше не нужно
            if (update.Message.Chat.Type is ChatType.Private)
                await MessageController.DeleteMessage(user, update.Message.MessageId);
            
            // Если пользователь заблокирован
            if (user.IsBlocked) return new IgnoreHandler();

            foreach (var handlerType in Commands)
            {
                var handler = (HandlerModel) Activator.CreateInstance(handlerType, user, update);
                if (handler.Match(user, update)) return handler;
            }
            
            return new IgnoreHandler();
        }

        protected internal override bool Match(UserEntity user, Update update)
        {
            return update.Message?.Type == MessageType.Text && update.Message?.Text == CommandText;
        }
        protected MessageHandler(UserEntity user, Update update) : base(user, update) { }
    }
}