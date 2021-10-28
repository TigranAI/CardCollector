using System;
using System.Threading.Tasks;
using CardCollector.Commands.Message;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    /* Реализует установку фильтров */
    public class SetFilter : CallbackQueryCommand
    {
        protected override string CommandText => Command.set;

        public override async Task Execute()
        {
            User.Session.UndoCurrentCommand();
            EnterEmoji.RemoveFromQueue(User.Id);
            var result = CallbackData.Split('=');
            var filters = User.Session.GetModule<FiltersModule>().Filters;
            /* Команду set мы получаем в виде set=<key>=<value>, соответственно аргументы 1 и 2 это ключ и значение словаря */
            filters[result[1]] = Convert.ChangeType(result[2], filters[result[1]].GetType());
            /* Если левая граница стоимости алмазов или монет больше правой, то меняем правую на бесконечность */
            if (filters[Command.price_coins_to] is int c && c <= (int) filters[Command.price_coins_from])
                filters[Command.price_coins_to] = 0;
            if (filters[Command.price_gems_to] is int g && g <= (int) filters[Command.price_gems_from])
                filters[Command.price_gems_to] = 0;
            /* Возвращаемся в меню фильтров */
            await new Back(User, Update).Execute();
        }
        
        public SetFilter() { }
        public SetFilter(UserEntity user, Update update) : base (user, update) { }
    }
}