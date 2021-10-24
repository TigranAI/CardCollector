using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Commands.InlineQuery;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using Telegram.Bot.Types;

namespace CardCollector.Commands
{
    /* Родительский класс для входящих обновлений типа InlineQuery (ввод пользователем @имя_бота)
     при наследовании укажите ключевое слово, содержащееся в запросе
     для поля Command и определите логику действий в Execute
     Также необходимо определить констуктор с параметрами UserEntity,
     Update и InlineQueryId, наслеуемый от base(user, update, inlineResult)
     И После реализации добавить команду в список List в этом классе
     Для обработки команды определены следующие поля
     User - пользователь, вызвавший команду
     Update - обновление, полученное от сервера Телеграм
     InlineQueryId - id запроса для ответа на него */
    public abstract class InlineQueryCommand : UpdateModel
    {
        /* Id входящего запроса */
        protected readonly string InlineQueryId;
        /* Запрос */
        protected readonly string Query;
        
        /* Список команд */
        private static readonly List<InlineQueryCommand> List = new()
        {
            // Показать стикеры в чатах для отправки (кроме личного чата с ботом)
            new ShowStickersInGroup(),
            // Показать стикеры на аукционе
            new ShowAuctionStickers(),
            // Показать стикеры в коллекции
            new ShowCollectionStickers(),
            // Показать стикеры для комбинации
            new ShowCombineStickers(),
            // Показать список продавцов
            new ShowTradersInBotChat(),
            // Показать стикеры в личных сообщениях с ботом для выбора или просмотра информации
            new ShowStickersInBotChat(),
            new ShowStickersInPrivate(),
        };
        
        
        /* Метод, создающий объекты команд исходя из полученного обновления */
        public static async Task<UpdateModel> Factory(Update update)
        {
            // Объект пользователя
            var user = await UserDao.GetUser(update.InlineQuery!.From);
            
            // Если пользователь заблокирован игонрируем
            if (user.IsBlocked) return new IgnoreUpdate();
            
            /* Возвращаем первую подходящую команду */
            return List.FirstOrDefault(item => item.IsMatches(user, update)) is { } executor
                ? (UpdateModel) Activator.CreateInstance(executor.GetType(), user, update)
                : new CommandNotFound(user, update, $"{update.InlineQuery!.ChatType}={update.InlineQuery!.Query}");
        }

        protected InlineQueryCommand() { }
        protected InlineQueryCommand(UserEntity user, Update update) : base(user, update)
        {
            InlineQueryId = update.InlineQuery!.Id;
            Query = update.InlineQuery!.Query;
        }
    }
}