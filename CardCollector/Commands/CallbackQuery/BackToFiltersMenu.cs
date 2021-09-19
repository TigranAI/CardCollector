using System.Threading.Tasks;
using CardCollector.Commands.Message;
using CardCollector.Commands.Message.TextMessage;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class BackToFiltersMenu : CallbackQuery
    {
        protected override string CommandText => Command.back;
        public override async Task Execute()
        {
            /* Удаляем пользователя из очереди */
            EnterEmojiMessage.RemoveFromQueue(User.Id);
            /* Формируем сообщение с имеющимися фильтрами у пользователя */
            var text = $"{Messages.current_filters}\n" +
                       $"{Messages.author} {(User.Filters[Command.author].Equals("") ? Messages.all : User.Filters[Command.author])}\n" +
                       $"{Messages.tier} {(User.Filters[Command.tier].Equals(-1) ? Messages.all : new string('⭐', (int) User.Filters[Command.tier]))}\n" +
                       $"{Messages.emoji} {(User.Filters[Command.emoji].Equals("") ? Messages.all : User.Filters[Command.emoji])}\n";
            if (User.State != UserState.CollectionMenu) 
                text += $"{Messages.price} 💰 {User.Filters[Command.price_coins_from]} -" +
                        $" {(User.Filters[Command.price_coins_to] is int c && c != 0 ? c : "∞")}\n" +
                        $"{Messages.price} 💎 {User.Filters[Command.price_gems_from]} -" +
                        $" {(User.Filters[Command.price_gems_to] is int g && g != 0 ? g : "∞")}\n";
            text += $"{Messages.sorting} {User.Filters[Command.sort]}\n\n{Messages.select_filter}";
            /* Редактируем сообщение */
            await MessageController.EditMessage(User, CallbackMessageId, text, Keyboard.GetSortingMenu(User.State));
        }
        
        public BackToFiltersMenu() { }
        public BackToFiltersMenu(UserEntity user, Update update) : base(user, update) { }
    }
}