using System.Threading.Tasks;
using CardCollector.Cache.Repository;
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
            await repo.SaveAsync(User.Id, Message.Chat.Id);
        }

        public override bool Match()
        {
            if (Message.Chat.Type is not (ChatType.Group or ChatType.Supergroup)) return false;
            return Message.ViaBot?.Username == AppSettings.NAME;
        }
    }
}