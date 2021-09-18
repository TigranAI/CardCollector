using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message
{
    public class EnterEmojiMessage : Message
    {
        protected override string CommandText => "";

        private const string oneEmojiPattern =
            "^\\u00a9$|^\\u00ae$|^[\\u2000-\\u3300]$|^\\ud83c[\\ud000-\\udfff]$|^\\ud83d[\\ud000-\\udfff]$|^\\ud83e[\\ud000-\\udfff]$";
        private const string onlyEmojiPattern =
            "\\u00a9|\\u00ae|[\\u2000-\\u3300]|\\ud83c[\\ud000-\\udfff]|\\ud83d[\\ud000-\\udfff]|\\ud83e[\\ud000-\\udfff]";
        
        /* Список пользователей, от которых ожидается ввод эмоджи ключ - id пользователя, значение - сообщение с меню */
        private static readonly Dictionary<long, int> Queue = new ();
        public override async Task Execute()
        {
            var input = "" + Update.Message!.Text; // это чтоб варн не показывлся, тут никогда не null, так как в Factory все уже проверено
            /* если пользователь ввел что-то кроме эмодзи */
            if (!Regex.IsMatch(input, onlyEmojiPattern))
                await MessageController.EditMessage(User, Queue[User.Id], Messages.please_enter_emoji, Keyboard.EmojiOptions);
            /* если пользователь ввел несколько эмодзи или эмодзи и текст */
            else if (!Regex.IsMatch(input, oneEmojiPattern))
                await MessageController.EditMessage(User, Queue[User.Id], Messages.enter_only_one_emoji, Keyboard.EmojiOptions);
            else
            {
                User.Filters[Command.emoji] = input;
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
                await MessageController.EditMessage(User, Queue[User.Id], text, Keyboard.GetSortingMenu(User.State));
                Queue.Remove(User.Id);
            }
        }

        /* Добавляем пользователя в очередь */
        public static void AddToQueue(long userId, int messageId)
        {
            Queue.TryAdd(userId, messageId);
        }

        /* Удаляем пользователя из очереди */
        public static void RemoveFromQueue(long userId)
        {
            Queue.Remove(userId);
        }

        /* Переопределяем метод, так как команда удовлетворяет условию, если пользователь находится в очереди */
        protected internal override bool IsMatches(string command)
        {
            return User == null ? base.IsMatches(command) : Queue.ContainsKey(User.Id);
        }

        public EnterEmojiMessage() { }
        public EnterEmojiMessage(UserEntity user, Update update) : base(user, update) { }
    }
}