using System.Threading.Tasks;
using CardCollector.Cache.Repository;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.MessageHandler.Group
{
    public class ChosenInlineResultInGroup : MessageHandler
    {
        protected override string CommandText => "";

        protected override async Task Execute()
        {
            var repo = new ChosenResultRepository();
            var chat = await Context.TelegramChats.FindByChatId(Message.Chat.Id);
            await repo.SaveAsync(User, chat.Id);
        }

        public override bool Match()
        {
            if (Message.Chat.Type is not (ChatType.Group or ChatType.Supergroup)) return false;
            return Message.ViaBot?.Username == AppSettings.NAME;
        }
    }
}