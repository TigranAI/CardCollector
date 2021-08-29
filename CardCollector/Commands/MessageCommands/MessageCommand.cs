using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using Telegram.Bot.Types;

namespace CardCollector.Commands.MessageCommands
{
    public abstract class MessageCommand : UpdateModel
    {
        private static readonly List<MessageCommand> List = new()
        {

        };

        public static async Task<MessageCommand> Factory(Update update)
        {
            // Текст команды
            var command = update.Message!.Text;
            
            // Id пользователя, отправившего команду
            var userId = update.Message!.From!.Id;
            // Объект пользователя
            var user = (await UserDao.GetById(userId)) ?? (await UserDao.AddNew(update.Message.From));
            
            // Удаляем предыдущие сообщения пользователя и бота
            MessageController.AddNewMessageToPool(update.Message.Chat.Id, update.Message.MessageId);
            await MessageController.DeleteMessagesFromPool(update.Message.Chat.Id);
            
            // Возвращаем объект, если команда совпала
            foreach (var item in List)
                if (item.IsMatches(command))
                    return (MessageCommand) Activator.CreateInstance(item.GetType(), user);
            
            // Возвращаем команда не найдена, если код дошел до сюда
            return new CommandNotFound(user);
        }

        protected MessageCommand(UserEntity user)
        {
            User = user;
        }
    }
}