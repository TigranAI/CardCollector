using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.Resources;

namespace CardCollector.DataBase.Entity
{
    public partial class UserEntity
    {
        public class UserSession
        {
            /* Текущее состояние пользователя */
            public UserState State = UserState.Default;

            /* Фильтры, примененные пользователем в меню коллекции/магазина/аукциона */
            public readonly Dictionary<string, object> Filters = new()
            {
                {Command.author, ""},
                {Command.tier, -1},
                {Command.emoji, ""},
                {Command.price_coins_from, 0},
                {Command.price_coins_to, 0},
                {Command.price_gems_from, 0},
                {Command.price_gems_to, 0},
                {Command.sort, SortingTypes.None},
            };
            
            /* Сообщения в чате пользователя */
            public readonly List<int> Messages = new();

            public async Task ClearMessages(UserEntity user)
            {
                foreach (var messageId in Messages)
                    await MessageController.DeleteMessage(user, messageId);
                Messages.Clear();
            }
        }
    }
}