using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using Telegram.Bot.Types;

namespace CardCollector.Commands.InlineQuery
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
    public abstract class InlineQuery : UpdateModel
    {
        /* Id входящего запроса */
        protected readonly string InlineQueryId = "";
        
        /* Список команд */
        private static readonly List<InlineQuery> List = new()
        {
            // Показать стикеры в чатах для отправки (кроме личного чата с ботом)
            new ShowStickersInGroup(),
            // Показать стикеры на аукционе
            new ShowAuctionStickers(),
            // Показать стикеры в коллекции
            new ShowCollectionStickers(),
            // Показать стикеры для комбинации
            new ShowCombineStickers(),
            // Показать стикеры в магазине
            new ShowShopStickers(),
            // Показать список продавцов
            new ShowTradersInBotChat(),
            // Показать стикеры в личных сообщениях с ботом для выбора или просмотра информации
            new ShowStickersInBotChat(),
            new ShowStickersInPrivate(),
        };
        
        
        /* Метод, создающий объекты команд исходя из полученного обновления */
        public static async Task<UpdateModel> Factory(Update update)
        {
            // Текст команды
            var command = $"{update.InlineQuery!.ChatType}={update.InlineQuery!.Query}";
            
            // Объект пользователя
            var user = await UserDao.GetUser(update.InlineQuery!.From);
            
            // Возвращаем объект, если команда совпала
            foreach (var item in List.Where(item => item.IsMatches(command)))
                if(Activator.CreateInstance(item.GetType(), user, update) is InlineQuery executor)
                    if (executor.IsMatches(command)) return executor;
        
            // Возвращаем команда не найдена, если код дошел до сюда
            return new CommandNotFound(user, update, command);
        }

        protected InlineQuery(UserEntity user, Update update) : base(user, update)
        {
            InlineQueryId = update.InlineQuery!.Id;
        }
        
        protected InlineQuery() { }
    }
}