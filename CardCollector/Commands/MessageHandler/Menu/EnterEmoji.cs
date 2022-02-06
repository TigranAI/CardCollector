using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.MessageHandler.Menu
{
    public class EnterEmoji : MessageHandler
    {
        protected override string CommandText => "";
        private const string ONLY_EMOJI_PATTERN =
            "\\u00a9|\\u00ae|[\\u2000-\\u3300]|\\ud83c[\\ud000-\\udfff]|\\ud83d[\\ud000-\\udfff]|\\ud83e[\\ud000-\\udfff]";
        
        private static readonly List<long> Queue = new ();
        protected override async Task Execute()
        {
            var input = Message.Text;
            if (!Regex.IsMatch(input!, ONLY_EMOJI_PATTERN))
                await User.Messages.EditMessage(User, Messages.please_enter_emoji, Keyboard.EmojiOptions);
            else
            {
                var filtersModule = User.Session.GetModule<FiltersModule>();
                filtersModule.Filters[Command.emoji] = input;
                var text = filtersModule.ToString(User.Session.State);
                await User.Messages.EditMessage(User, text, Keyboard.GetSortingMenu(User.Session.State));
                Queue.Remove(User.Id);
            }
        }

        public static void AddToQueue(long userId)
        {
            if (!Queue.Contains(userId)) Queue.Add(userId);
        }

        public static void RemoveFromQueue(long userId)
        {
            Queue.Remove(userId);
        }

        public override bool Match()
        {
            return Queue.Contains(User.Id) && Message!.Type == MessageType.Text;
        }

        public EnterEmoji(User user, BotDatabaseContext context, Message message) : base(user, context, message)
        {
        }
    }
}