using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Commands.PreCheckoutQuery;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using Telegram.Bot.Types;

namespace CardCollector.Commands
{
    public abstract class PreCheckoutQueryCommand : UpdateModel
    {
        protected readonly string PreCheckoutQueryId;
        protected readonly int Amount;
        
        private static readonly List<PreCheckoutQueryCommand> List = new()
            {
                // Тестовая покупка
                new Test(),
                // Покупка 50 алмазов
                new BuyGems(),
            };

        /* Метод, создающий объекты команд исходя из полученного обновления */
        public static async Task<UpdateModel> Factory(Update update)
        {
            // Объект пользователя
            var user = await UserDao.GetUser(update.PreCheckoutQuery!.From);
            
            // Если пользователь заблокирован игонрируем
            if (user.IsBlocked) return new IgnoreUpdate();
            
            // Возвращаем объект, если команда совпала
            return List.FirstOrDefault(item => item.IsMatches(user, update)) is { } executor
                ? (UpdateModel) Activator.CreateInstance(executor.GetType(), user, update)
                : new CommandNotFound(user, update, update.PreCheckoutQuery!.InvoicePayload);
        }

        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            return CommandText == update.PreCheckoutQuery!.InvoicePayload;
        }

        protected PreCheckoutQueryCommand() { }
        protected PreCheckoutQueryCommand(UserEntity user, Update update) : base(user, update)
        {
            PreCheckoutQueryId = update.PreCheckoutQuery!.Id;
            Amount = update.PreCheckoutQuery.TotalAmount;
        }
    }
}