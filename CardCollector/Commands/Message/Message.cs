using System;
using System.Threading.Tasks;
using CardCollector.Controllers;
using System.Collections.Generic;
using System.Linq;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message
{
    using static Logs;
    public abstract class Message : UpdateModel
    {
        private static readonly List<Message> List = new()
        {
            new ProfileMessage(),
            new StartMessage()
        };

        public static async Task<UpdateModel> Factory(Update update)
        {
            try
            {
                // Текст команды
                var command = update.Message!.Text;
            
                // Id пользователя, отправившего команду
                var userId = update.Message!.From!.Id;
                // Объект пользователя
                var user = await UserDao.GetById(userId) ?? await UserDao.AddNew(update.Message.From);
            
                // Добавляем сообщения пользователя в пул для удаления
                MessageController.AddNewMessageToPool(user, update.Message.MessageId);
                
                // Возвращаем объект, если команда совпала
                foreach (var item in List.Where(item => item.IsMatches(command)))
                    if(Activator.CreateInstance(item.GetType(), user, update) is Message executor)
                        if (executor.IsMatches(command)) return executor;
            
                // Возвращаем команда не найдена, если код дошел до сюда
                return new CommandNotFound(user, update, command);
            }
            catch (Exception e)
            {
                LogOutError(e);
                throw;
            }
        }

        protected Message(UserEntity user, Update update) : base(user, update) { }
        protected Message() { }
    }
}