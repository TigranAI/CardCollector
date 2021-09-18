using System;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    /* Реализует установку фильтров */
    public class SetFilterCallback : CallbackQuery
    {
        protected override string CommandText => Command.set;
        public override async Task Execute()
        {
            var result = CallbackData.Split('=');
            /* Команду set мы получаем в виде set=<key>=<value>, соответственно аргументы 1 и 2 это ключ и значение словаря */
            User.Filters[result[1]] = Convert.ChangeType(result[2], User.Filters[result[1]].GetType());
            /* Возвращаемся в меню фильтров */
            await new BackToFiltersMenu(User, Update).Execute();
        }
        
        public SetFilterCallback() { }
        public SetFilterCallback(UserEntity user, Update update) : base (user, update) { }
    }
}