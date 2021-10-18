using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message.TextMessage
{
    public class StopBot : Message
    {
        protected override string CommandText => Text.stop_bot;
        
        public override async Task Execute()
        {
            var message = await MessageController.SendMessage(User, "Stopping bot");
            User.Session.Messages.Add(message.MessageId);
            Bot.StopProgram();
        }

        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            return base.IsMatches(user, update) && User.PrivilegeLevel >= Constants.PROGRAMMER_PRIVILEGE_LEVEL;
        }

        public StopBot() { }
        public StopBot(UserEntity user, Update update) : base(user, update) { }
    }
}