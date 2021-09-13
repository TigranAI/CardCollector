using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using Telegram.Bot.Types;

namespace CardCollector.Commands.InlineQuery
{
    public abstract class InlineQuery : UpdateModel
    {
        protected readonly string InlineQueryId = "";
        
        private static readonly List<InlineQuery> List = new()
        {
            new EmptyInlineQuery(),
            //new FilteredInlineQuery(),
        };
        
        public static async Task<UpdateModel> Factory(Update update)
        {
            try
            {
                // Текст команды
                var command = update.InlineQuery!.Query;
                
                // Объект пользователя
                var user = await UserDao.GetUser(update.InlineQuery!.From);
                
                // Возвращаем объект, если команда совпала
                foreach (var item in List.Where(item => item.IsMatches(command)))
                    if(Activator.CreateInstance(item.GetType(), user, update, update.InlineQuery.Id) is InlineQuery executor)
                        if (executor.IsMatches(command)) return executor;
            
                // Возвращаем команда не найдена, если код дошел до сюда
                return new CommandNotFound(user, update, command);
            }
            catch (Exception e)
            {
                Logs.LogOutError(e);
                throw;
            }
        }

        protected InlineQuery(UserEntity user, Update update, string inlineQueryId) : base(user, update)
        {
            InlineQueryId = inlineQueryId;
        }
        
        protected InlineQuery() { }
    }
}