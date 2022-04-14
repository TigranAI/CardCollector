using System.Threading.Tasks;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Others;
using CardCollector.Resources.Translations;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.MessageHandler.Group
{
    public class EnableDistributions : MessageHandler
    {
        protected override string CommandText => MessageCommands.enable_distributions;
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
            if (chatMember.Status is not (ChatMemberStatus.Administrator or ChatMemberStatus.Creator))
                await telegramChat.SendMessage(Messages.this_command_can_execute_only_administrator);
            else
            {
                telegramChat.DistributionsDisabled = false;
                await telegramChat.SendMessage(Messages.distributions_successfully_enabled);
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