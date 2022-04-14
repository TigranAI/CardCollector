using System.Threading.Tasks;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Others;
using CardCollector.Resources.Translations;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.MessageHandler.Group
{
    public class DisableDistributions : MessageHandler
    {
        protected override string CommandText => MessageCommands.disable_distributions;
        protected override async Task Execute()
        {
            var telegramChat = await Context.TelegramChats.FindByChatId(Message.Chat.Id);
            if (telegramChat == null)
            {
                var result = await Context.TelegramChats.AddAsync(new TelegramChat()
                {
                    ChatId = Message.Chat.Id,
                    ChatActivity = new ChatActivity(),
                    ChatType = Message.Chat.Type,
                    Title = Message.Chat.Title
                });
                telegramChat = result.Entity;
            }
            var chatMember = await Bot.Client.GetChatMemberAsync(Message.Chat.Id, User.ChatId);
            Logs.LogOut(chatMember.Status);
            if (chatMember.Status is not (ChatMemberStatus.Administrator or ChatMemberStatus.Creator))
                await telegramChat.SendMessage(Messages.this_command_can_execute_only_administrator);
            else
            {
                telegramChat.DistributionsDisabled = true;
                await telegramChat.SendMessage(Messages.distributions_successfully_disabled);
            }
        }
        
        public override bool Match()
        {
            if (Message.Type != MessageType.Text) return false;
            if (Message.Text!.Split("@")[0] != CommandText) return false;
            return Message.Chat.Type is ChatType.Group or ChatType.Supergroup;
        }
    }
}