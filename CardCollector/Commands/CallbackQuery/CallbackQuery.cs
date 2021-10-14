using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    /* Родительский класс для входящих обновлений типа CallbackQuery (нажатие пользователем инлайн кнопки)
     при наследовании укажите ключевое слово, содержащееся в запросе
     для поля Command и определите логику действий в Execute
     Также необходимо определить констуктор с параметрами UserEntity
     и Update, наслеуемый от base(user, update)
     И После реализации добавить команду в список List в этом классе
     Для обработки команды определены следующие поля
     User - пользователь, вызвавший команду
     Update - обновление, полученное от сервера Телеграм */
    public abstract class CallbackQuery : UpdateModel
    {
        /* Данные, поступившие после нажатия на кнокпку */
        protected string CallbackData;
        
        /* Id сообщения, под которым нажали на кнопку */
        protected int CallbackMessageId;
        
        /* Id запроса */
        protected string CallbackQueryId;
        
        /* Список команд, распознаваемых ботом */
        private static readonly List<CallbackQuery> List = new()
        {
            /* Кнопка "Автор" */
            new AuthorCallback(),
            /* Кнопка "Тир" */
            new TierCallback(),
            /* Кнопка "Эмоция" */
            new EmojiCallback(),
            /* Кнопка "Цена" */
            new PriceCallback(),
            /* Кнопка "Сортировка" */
            new SortCallback(),
            /* Установка фильтра */
            new SetFilterCallback(),
            /* Команда отмены */
            new CancelCallback(),
            /* Команда отмены */
            new CollectIncomeQuery(),
            /* Команда для упралвения количеством стикеров */
            new CountQuery(),
            /* Команда для подтверждения покупки */
            new ConfirmBuyingQuery(),
            /* команда для подтверждения выставления на аукцион */
            new PutForAuctionQuery(),
            /* Команда для покупки стикера */
            new BuyStickerQuery(),
            /* Команда возврата к комбинированию */
            new BackToCombine(),
            /* Команда добавления к комбинироавнию */
            new CombineCallback(),
            /* Команда удаления из комбинации */
            new DeleteCombine(),
            /* Команда удаления из комбинации */
            new CombineStickers(),
            /* Команда которая позволяет пользователю просмотреть паки */
            new MyPacksQuery(),
            /* Показывает меню по автору */
            new AuthorMenuQuery(),
            /* Открытие рандомного пака */
            new OpenRandomQuery(),
            /* Открытие пака автора */
            new OpenPackCallback(),
            /* Команда которая показывает список ежедневных заданий */
            new DailyTasksQuery(),
            //команда подтверждения выставления на аукцион
            new ConfirmationSellingQuery(),
            /* Отмена в момент выбора "значения фильтра", не в самом меню */
            new BackToFiltersMenu(),
            /* Очистка чата */
            new ClearChat(),
        };

        /* Метод, создающий объекты команд исходя из полученного обновления */
        public static async Task<UpdateModel> Factory(Update update)
        {
            // Текст команды
            var command = update.CallbackQuery!.Data;

            // Объект пользователя
            var user = await UserDao.GetUser(update.CallbackQuery.From);

            // Возвращаем объект, если команда совпала
            foreach (var item in List.Where(item => item.IsMatches(command)))
                if (Activator.CreateInstance(item.GetType(), user, update) is CallbackQuery executor && executor.IsMatches(command))
                    return executor;

            // Возвращаем команда не найдена, если код дошел до сюда
            return new CommandNotFound(user, update, command);
        }

        protected internal override bool IsMatches(string command)
        {
            var query = command.Split('=')[0];
            return base.IsMatches(query);
        }

        protected CallbackQuery(UserEntity user, Update update) : base(user, update)
        {
            CallbackData = update.CallbackQuery!.Data;
            CallbackMessageId = update.CallbackQuery!.Message!.MessageId;
            CallbackQueryId = update.CallbackQuery!.Id;
        }

        protected CallbackQuery() { }
    }
}