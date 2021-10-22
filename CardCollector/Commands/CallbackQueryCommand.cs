using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Commands.CallbackQuery;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using Telegram.Bot.Types;

namespace CardCollector.Commands
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
    public abstract class CallbackQueryCommand : UpdateModel
    {
        /* Данные, поступившие после нажатия на кнокпку */
        protected string CallbackData;
        
        /* Id сообщения, под которым нажали на кнопку */
        protected int CallbackMessageId;
        
        /* Id запроса */
        protected string CallbackQueryId;
        
        /* Список команд, распознаваемых ботом */
        private static readonly List<CallbackQueryCommand> List = new()
        {
            new SelectAuthor(),
            new AuthorMenu(),
            new BackToCombine(),
            new BackToFiltersMenu(),
            new BuyByCoins(),
            new BuyByGems(),
            new BuyCoins(),
            new BuyGems(),
            new BuyPack(),
            new BuySticker(),
            new Cancel(),
            new ClearChat(),
            new CollectIncome(),
            new Combine(),
            new CombineStickers(),
            new ConfirmationSelling(),
            new ConfirmBuying(),
            new ConfirmExchange(),
            new Count(),
            new CallbackQuery.DailyTasks(),
            new DeleteCombine(),
            new SelectEmoji(),
            new MyPacks(),
            new OpenPack(),
            new OpenSpecific(),
            new PackInfo(),
            new SelectPrice(),
            new PutForAuction(),
            new SelectOffer(),
            new SetFilter(),
            new SelectSort(),
            new ShowInfo(),
            new SpecialOffers(),
            new SelectTier(),
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

        protected CallbackQueryCommand() { }
        protected CallbackQueryCommand(UserEntity user, Update update) : base(user, update)
        {
            CallbackData = update.CallbackQuery!.Data;
            CallbackMessageId = update.CallbackQuery!.Message!.MessageId;
            CallbackQueryId = update.CallbackQuery!.Id;
        }
    }
}