using System;
using System.Threading.Tasks;
using CardCollector.Commands.Message;
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
            EnterEmojiMessage.RemoveFromQueue(User.Id);
            var result = CallbackData.Split('=');
            /* Команду set мы получаем в виде set=<key>=<value>, соответственно аргументы 1 и 2 это ключ и значение словаря */
            User.Filters[result[1]] = Convert.ChangeType(result[2], User.Filters[result[1]].GetType());
            /* Если левая граница стоимости алмазов или монет больше правой, то меняем правую на бесконечность */
            if (User.Filters[Command.price_coins_to] is int c && c <= (int) User.Filters[Command.price_coins_from])
                User.Filters[Command.price_coins_to] = 0;
            if (User.Filters[Command.price_gems_to] is int g && g <= (int) User.Filters[Command.price_gems_from])
                User.Filters[Command.price_gems_to] = 0;
            /* Возвращаемся в меню фильтров */
            await new BackToFiltersMenu(User, Update).Execute();
        }
        
        public SetFilterCallback() { }
        public SetFilterCallback(UserEntity user, Update update) : base (user, update) { }
    }
}