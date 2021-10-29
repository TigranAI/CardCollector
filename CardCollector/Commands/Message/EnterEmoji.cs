using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.Message
{
    public class EnterEmoji : MessageCommand
    {
        protected override string CommandText => "";

        private const string oneEmojiPattern =
            "^\\u00a9$|^\\u00ae$|^[\\u2000-\\u3300]$|^\\ud83c[\\ud000-\\udfff]$|^\\ud83d[\\ud000-\\udfff]$|^\\ud83e[\\ud000-\\udfff]$";
        private const string onlyEmojiPattern =
            "\\u00a9|\\u00ae|[\\u2000-\\u3300]|\\ud83c[\\ud000-\\udfff]|\\ud83d[\\ud000-\\udfff]|\\ud83e[\\ud000-\\udfff]";
        
        private static readonly List<long> Queue = new ();
        public override async Task Execute()
        {
            var input = Update.Message!.Text;
            /* если пользователь ввел что-то кроме эмодзи */
            if (!Regex.IsMatch(input!, onlyEmojiPattern))
                await MessageController.EditMessage(User, Messages.please_enter_emoji, Keyboard.EmojiOptions);
            /* если пользователь ввел несколько эмодзи или эмодзи и текст */
            else if (!Regex.IsMatch(input, oneEmojiPattern))
                await MessageController.EditMessage(User, Messages.enter_only_one_emoji,
                    Keyboard.EmojiOptions);
            else
            {
                var filtersModule = User.Session.GetModule<FiltersModule>();
                filtersModule.Filters[Command.emoji] = input;
                /* Формируем сообщение с имеющимися фильтрами у пользователя */
                var text = filtersModule.ToString(User.Session.State);
                /* Редактируем сообщение */
                await MessageController.EditMessage(User, text, Keyboard.GetSortingMenu(User.Session.State));
                Queue.Remove(User.Id);
            }
        }

        /* Добавляем пользователя в очередь */
        public static void AddToQueue(long userId)
        {
            if (!Queue.Contains(userId)) Queue.Add(userId);
        }

        /* Удаляем пользователя из очереди */
        public static void RemoveFromQueue(long userId)
        {
            Queue.Remove(userId);
        }

        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            return Queue.Contains(user.Id) && update.Message!.Type == MessageType.Text;
        }

        public EnterEmoji() { }
        public EnterEmoji(UserEntity user, Update update) : base(user, update) { }
    }
}