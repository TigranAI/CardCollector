using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.MessageHandler.UserToChat
{
    public class SendStickerInChat : MessageHandler
    {
        protected override string CommandText => "";

        protected override async Task Execute()
        {
            User.Session.ChosenResultWithMessage = true;
            var countSentStickers = await Context.UserSendStickers
                .Where(item => item.User.Id == User.Id && item.ChatId == Message.Chat.Id)
                .CountAsync();
            if (countSentStickers < 5)
            {
                var membersCount = await Bot.Client.GetChatMemberCountAsync(Message.Chat.Id) - 1;
                User.Level.GiveExp(membersCount < 21 ? membersCount : 20);
                if (User.Settings[UserSettingsEnum.ExpGain])
                    await User.Messages.SendMessage(User,
                        $"{Messages.you_gained} {(membersCount < 21 ? membersCount : 20)} {Text.exp} {Messages.send_sticker}" +
                        $"\n{Messages.count_sends_per_day} \"{Message.Chat.Title}\" {countSentStickers + 1} / 5");
                await User.Level.CheckLevelUp(Context, User);
                await Context.UserSendStickers.AddAsync(new UserSendStickerToChat()
                    {User = User, ChatId = Message.Chat.Id});
            }
        }

        public override bool Match()
        {
            if (Message.ViaBot is not { } bot) return false;
            if (bot.Username != AppSettings.NAME) return false;
            if (Message.Type != MessageType.Sticker) return false;
            return Message.Chat.Type is ChatType.Group or ChatType.Supergroup;
        }

        public SendStickerInChat(User user, BotDatabaseContext context, Message message) : base(user, context, message)
        {
        }
    }
}