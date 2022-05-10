using System;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.MessageHandler.Group
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
                var telegramChat = await Context.TelegramChats.FindByChatId(Message.Chat.Id);
                var membersCount = await Bot.Client.GetChatMemberCountAsync(Message.Chat.Id) - 1;
                User.Level.GiveExp(Math.Min(membersCount, telegramChat.MaxExpGain));
                if (User.Settings[UserSettingsTypes.ExpGain])
                    await User.Messages.SendMessage(User,
                        $"{Messages.you_gained} {Math.Min(membersCount, telegramChat.MaxExpGain)} " +
                        $"{Text.exp} {Messages.send_sticker}" +
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
    }
}