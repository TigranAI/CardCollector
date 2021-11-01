using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.Message
{
    public class GiveExp : MessageCommand
    {
        protected override string CommandText => "";

        public static readonly Dictionary<long, Dictionary<long, int>> GroupStickersExp = new();

        public override async Task Execute()
        {
            if (Update.Message == null) return;
            User.Session.ChosenResultWithMessage = true;
            var chatId = Update.Message.Chat.Id;
            if (!GroupStickersExp.ContainsKey(chatId))
                GroupStickersExp.Add(chatId, new Dictionary<long, int>());
            if (!GroupStickersExp[chatId].ContainsKey(User.Id))
                GroupStickersExp[chatId].Add(User.Id, 0);
            if (GroupStickersExp[chatId][User.Id] < 5)
            {
                GroupStickersExp[chatId][User.Id]++;
                var membersCount = await Bot.Client.GetChatMemberCountAsync(chatId) - 1;
                await User.GiveExp(membersCount < 21 ? membersCount : 20);
                if (User.Settings[UserSettingsEnum.ExpGain])
                    await MessageController.SendMessage(User, 
                        $"{Messages.you_gained} {(membersCount < 21 ? membersCount : 20)} {Text.exp} {Messages.send_sticker}" +
                        $"\n{Messages.count_sends_per_day} \"{Update.Message.Chat.Title}\" {GroupStickersExp[chatId][User.Id]} / 5");
            }
        }

        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            return update.Message?.ViaBot is { } bot && bot.Username == AppSettings.NAME &&
                   update.Message.Type is MessageType.Sticker &&
                   update.Message.Chat.Type is ChatType.Group or ChatType.Supergroup;
        }

        public GiveExp() { }
        public GiveExp(UserEntity user, Update update) : base(user, update) { }
    }
}