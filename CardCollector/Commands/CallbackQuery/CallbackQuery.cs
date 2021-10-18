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
            new AuthorCallback(),
            new AuthorMenuQuery(),
            new BackToCombine(),
            new BackToFiltersMenu(),
            new BuyByCoins(),
            new BuyByGems(),
            new BuyGemsQuery(),
            new BuyPackQuery(),
            new BuyStickerQuery(),
            new CancelCallback(),
            new ClearChat(),
            new CollectIncomeQuery(),
            new CombineCallback(),
            new CombineStickers(),
            new ConfirmationSellingQuery(),
            new ConfirmBuyingQuery(),
            new CountQuery(),
            new DailyTasksQuery(),
            new DeleteCombine(),
            new EmojiCallback(),
            new MyPacksQuery(),
            new OpenPackCallback(),
            new OpenSpecificCallback(),
            new PackInfo(),
            new PriceCallback(),
            new PutForAuctionQuery(),
            new SelectOfferCallback(),
            new SetFilterCallback(),
            new SortCallback(),
            new TierCallback(),
        };

        /* Метод, создающий объекты команд исходя из полученного обновления */
        public static async Task<UpdateModel> Factory(Update update)
        {
            // Объект пользователя
            var user = await UserDao.GetUser(update.CallbackQuery!.From);
            
            // Если пользователь заблокирован игонрируем
            if (user.IsBlocked) return new IgnoreUpdate();

            // Возвращаем объект, если команда совпала
            return List.FirstOrDefault(item => item.IsMatches(user, update)) is { } executor
                ? (UpdateModel) Activator.CreateInstance(executor.GetType(), user, update)
                : new CommandNotFound(user, update, update.CallbackQuery!.Data);
        }

        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            var query = update.CallbackQuery!.Data!.Split('=')[0];
            return query == CommandText;
        }

        protected CallbackQuery() { }
        protected CallbackQuery(UserEntity user, Update update) : base(user, update)
        {
            CallbackData = update.CallbackQuery!.Data;
            CallbackMessageId = update.CallbackQuery!.Message!.MessageId;
            CallbackQueryId = update.CallbackQuery!.Id;
        }
    }
}