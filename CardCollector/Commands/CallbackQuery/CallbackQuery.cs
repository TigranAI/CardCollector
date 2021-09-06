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
    using static Logs;
    public abstract class CallbackQuery : UpdateModel
    {
        
        private static readonly List<CallbackQuery> List = new()
        {

        };

        public static async Task<UpdateModel> Factory(Update update)
        {
            try
            {
                // Текст команды
                var command = update.CallbackQuery.Data;

                // Объект пользователя
                var user = await UserDao.GetOrAddNew(update.CallbackQuery.From);

                // Добавляем сообщения пользователя в пул для удаления
                MessageController.AddNewMessageToPool(user, update.CallbackQuery.Message.MessageId);

                // Возвращаем объект, если команда совпала
                foreach (var item in List.Where(item => item.IsMatches(command)))
                    if (Activator.CreateInstance(item.GetType(), user, update) is CallbackQuery executor && executor.IsMatches(command))
                        return executor;

                // Возвращаем команда не найдена, если код дошел до сюда
                return new CommandNotFound(user, update, command);
            }
            catch (Exception e)
            {
                LogOutError(e);
                throw;
            }
        }

        protected CallbackQuery(UserEntity user, Update update) : base(user, update) { }
    }
}