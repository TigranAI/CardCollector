using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using Telegram.Bot.Types;

namespace CardCollector.Commands.ChosenInlineResult
{
    public abstract class ChosenInlineResult : UpdateModel
    {
        protected readonly string InlineResult = "";
        
        private static readonly List<ChosenInlineResult> List = new()
        {
            //new StickerInlineResult(),
        };
        
        public static async Task<UpdateModel> Factory(Update update)
        {
            try
            {
                // Текст команды
                var command = update.ChosenInlineResult!.ResultId;
                
                // Объект пользователя
                var user = await UserDao.GetUser(update.ChosenInlineResult!.From);
                
                // Возвращаем объект, если команда совпала
                foreach (var item in List.Where(item => item.IsMatches(command)))
                    if(Activator.CreateInstance(item.GetType(), 
                        user, update, update.ChosenInlineResult.ResultId) is ChosenInlineResult executor)
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

        protected ChosenInlineResult(UserEntity user, Update update, string inlineResult)
            : base(user, update)
        {
            InlineResult = inlineResult;
        }

        protected ChosenInlineResult() { }
    }
}