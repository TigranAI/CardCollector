using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Custom
{
    public class BackToFiltersMenu : CallbackQuery.CallbackQuery
    {
        protected override string Command => "CallbackQuery";
        public override async Task Execute()
        {
            /* Формируем сообщение с имеющимися фильтрами у пользователя */
            var text = $"{Messages.current_filters}\n" +
                       $"{Messages.author} {(User.Filters["author"].Equals("") ? Messages.all : User.Filters["author"])}\n" +
                       $"{Messages.tier} {(User.Filters["tier"].Equals(-1) ? Messages.all : new string('⭐', (int)User.Filters["tier"]))}\n" +
                       $"{Messages.emoji} {(User.Filters["emoji"].Equals("") ? Messages.all : User.Filters["emoji"])}\n" +
                       $"{Messages.price} {User.Filters["price"]}-∞\n" +
                       $"{Messages.sorting} {User.Filters["sorting"]}\n" +
                       $"\n{Messages.select_filter}";
            /* Редактируем сообщение */
            await MessageController.EditMessage(User, CallbackMessageId, text, Keyboard.SortingOptions);
        }
        
        public BackToFiltersMenu(UserEntity user, Update update) : base(user, update) { }
    }
}