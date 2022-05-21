using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.MessageHandler.Menu
{
    public class EnterEmoji : MessageHandler
    {
        protected override string CommandText => "";
        private const string ONLY_EMOJI_PATTERN =
            "\\u00a9|\\u00ae|[\\u2000-\\u3300]|\\ud83c[\\ud000-\\udfff]|\\ud83d[\\ud000-\\udfff]|\\ud83e[\\ud000-\\udfff]";
        
        private static readonly LinkedList<long> Queue = new ();
        protected override async Task Execute()
        {
            var input = Message.Text;
            if (!Regex.IsMatch(input!, ONLY_EMOJI_PATTERN))
                await User.Messages.EditMessage(Messages.please_enter_emoji, Keyboard.EmojiOptions);
            else
            {
                var filtersModule = User.Session.GetModule<FiltersModule>();
                filtersModule.Emoji = input;
                var text = filtersModule.ToString(User.Session.State);
                await User.Messages.EditMessage(text, Keyboard.GetSortingMenu(User.Session.State));
                Queue.Remove(User.Id);
            }
        }

        public static void AddToQueue(long userId)
        {
            if (!Queue.Contains(userId)) Queue.AddLast(userId);
        }

        public static void RemoveFromQueue(long userId)
        {
            Queue.Remove(userId);
        }

        public override bool Match()
        {
            return Queue.Contains(User.Id) && Message!.Type == MessageType.Text;
        }
    }
}