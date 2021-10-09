using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using Telegram.Bot.Types;

namespace CardCollector.Commands.ChosenInlineResult
{
    /* Родительский класс для входящих обновлений типа ChosenInlineResult
     (выбор пользователем инлайн команды)
     при наследовании укажите ключевое слово, содержащееся в запросе
     для поля Command и определите логику действий в Execute
     Также необходимо определить констуктор с параметрами UserEntity,
     Update и InlineResult, наслеуемый от base(user, update, inlineResult)
     И После реализации добавить команду в список List в этом классе
     Для обработки команды определены следующие поля
     User - пользователь, вызвавший команду
     Update - обновление, полученное от сервера Телеграм
     InlineResult - результат входящего запроса */
    public abstract class ChosenInlineResult : UpdateModel
    {
        /* Результат запроса (id выбранного пользователем элемента) */
        protected readonly string InlineResult = "";
        
        /* Список команд */
        protected static readonly List<ChosenInlineResult> List = new()
        {
            /* Этот объект должен быть всегда в начале списка, так как он должен быть вызван
             вперед других, если в коде включен режим бесконечных стикеров */
            new GetUnlimitedStickerAndExecuteCommand(),
        
            // Обработка результата при отправке стикера
            new SendStickerResult(),
            new SendPrivateSticker(),
            // Обработка результата при выборе продавца
            new SelectTraderResult(),
            
            new SelectStickerInlineResult(),
        };
        
        /* Метод, создающий объекты команд исходя из полученного обновления */
        public static async Task<UpdateModel> Factory(Update update)
        {
            // Текст команды
            var command = update.ChosenInlineResult!.ResultId;
            
            // Объект пользователя
            var user = await UserDao.GetUser(update.ChosenInlineResult!.From);
            
            // Возвращаем объект, если команда совпала
            foreach (var item in List.Where(item => item.IsMatches(command)))
                if(Activator.CreateInstance(item.GetType(), user, update) is ChosenInlineResult executor)
                    if (executor.IsMatches(command)) return executor;
        
            // Возвращаем команда не найдена, если код дошел до сюда
            return new CommandNotFound(user, update, command);
        }

        protected ChosenInlineResult(UserEntity user, Update update) : base(user, update)
        {
            InlineResult = update.ChosenInlineResult!.ResultId;
        }

        protected ChosenInlineResult() { }
    }
}