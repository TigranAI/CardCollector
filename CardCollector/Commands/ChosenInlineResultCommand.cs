using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Commands.ChosenInlineResult;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using Telegram.Bot.Types;

namespace CardCollector.Commands
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
    public abstract class ChosenInlineResultCommand : UpdateModel
    {
        /* Результат запроса (id выбранного пользователем элемента) */
        protected readonly string InlineResult;

        /* Команда запроса */
        protected readonly string InlineQuery;

        /* Список команд */
        protected static readonly List<ChosenInlineResultCommand> List = new()
        {
            /* Этот объект должен быть всегда в начале списка, так как он должен быть вызван
             вперед других, если в коде включен режим бесконечных стикеров */
            new GetUnlimitedStickerAndExecuteCommand(),

            // Обработка результата при отправке стикера
            new SendSticker(),
            new SendPrivateSticker(),
            // Обработка результата при выборе продавца
            new SelectTrader(),
            new StickerInfo(),
            
            new SelectStickerInline(),
        };

        /* Метод, создающий объекты команд исходя из полученного обновления */
        public static async Task<UpdateModel> Factory(Update update)
        {
            // Объект пользователя
            var user = await UserDao.GetUser(update.ChosenInlineResult!.From);
            
            // Если пользователь заблокирован игонрируем
            if (user.IsBlocked) return new IgnoreUpdate();

            // Возвращаем объект, если команда совпала
            return List.FirstOrDefault(item => item.IsMatches(user, update)) is { } executor
                ? (UpdateModel) Activator.CreateInstance(executor.GetType(), user, update)
                : new CommandNotFound(user, update, update.ChosenInlineResult.ResultId);
        }

        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            var query = update.ChosenInlineResult!.ResultId.Split("=")[0];
            return CommandText == query;
        }

        protected ChosenInlineResultCommand() { }
        protected ChosenInlineResultCommand(UserEntity user, Update update) : base(user, update)
        {
            InlineResult = update.ChosenInlineResult!.ResultId;
            InlineQuery = update.ChosenInlineResult.Query;
        }
    }
}