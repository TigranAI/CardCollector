using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.MessageHandler.Admin.Giveaway
{
    public class EnterSendDatetime : MessageHandler
    {
        protected override string CommandText => "";

        private static readonly LinkedList<long> Queue = new();

        protected override async Task Execute()
        {
            if (DateTime.TryParseExact(Message.Text, "dd.MM HH:mm", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var datetime))
            {
                RemoveFromQueue(User.Id);
                var module = User.Session.GetModule<AdminModule>();
                if (module.SelectedChannelGiveawayId == null) return;
                var giveaway = await Context.ChannelGiveaways.FindById(module.SelectedChannelGiveawayId.Value);
                giveaway.SendAt = datetime;
                await User.Messages.EditMessage(User, Messages.enter_button_text,
                    Keyboard.SkipKeyboard(typeof(EnterButtonText).Name), ParseMode.Html);
                EnterButtonText.AddToQueue(User.Id);
            }
            else await User.Messages.EditMessage(User, Messages.incorrect_datetime_format,
                    Keyboard.SkipKeyboard(GetType().Name));
        }

        public static async Task Skip(User user, BotDatabaseContext botDatabaseContext)
        {
            RemoveFromQueue(user.Id);
            await user.Messages.EditMessage(user, Messages.enter_button_text,
                Keyboard.SkipKeyboard(typeof(EnterButtonText).Name), ParseMode.Html);
            EnterButtonText.AddToQueue(user.Id);
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
            return Queue.Contains(User.Id) && Message.Type == MessageType.Text;
        }

        public EnterSendDatetime(User user, BotDatabaseContext context, Message message) : base(user, context, message)
        {
        }
    }
}