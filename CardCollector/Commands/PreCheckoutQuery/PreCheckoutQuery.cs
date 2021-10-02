using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using Telegram.Bot.Types;

namespace CardCollector.Commands.PreCheckoutQuery
{
    public abstract class PreCheckoutQuery : UpdateModel
    {
        protected readonly string PreCheckoutQueryId;
        
        private static readonly List<PreCheckoutQuery> List = new()
            {
                // Тестовая покупка
                new TestPreCheckoutQuery(),
            };

        /* Метод, создающий объекты команд исходя из полученного обновления */
        public static async Task<UpdateModel> Factory(Update update)
        {
            /* Данные определяем исходя из указанного нами продукта */
            var data = update.PreCheckoutQuery!.InvoicePayload;
            
            // Объект пользователя
            var user = await UserDao.GetUser(update.PreCheckoutQuery!.From);
            
            // Если пользователь заблокирован игонрируем
            if (user.IsBlocked) return new IgnoreUpdate();
            
            // Возвращаем объект, если команда совпала
            foreach (var item in List.Where(item => item.IsMatches(data)))
                if(Activator.CreateInstance(item.GetType(), user, update) is PreCheckoutQuery executor)
                    if (executor.IsMatches(data)) return executor;
        
            // Возвращаем команда не найдена, если код дошел до сюда
            return new CommandNotFound(user, update, data);
        }

        protected PreCheckoutQuery() { }
        protected PreCheckoutQuery(UserEntity user, Update update) : base(user, update)
        {
            PreCheckoutQueryId = update.PreCheckoutQuery!.Id;
        }
    }
}