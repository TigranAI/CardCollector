using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.MessageHandler.UrlCommands
{
    public abstract class MessageUrlHandler : MessageHandler
    {
        protected string[] StartData;
        
        public override bool Match()
        {
            if (Message.Type is not MessageType.Text) return false;
            var command = Message.Text!.Split(" ");
            if (command[0] != MessageCommands.start || command.Length == 1) return false;
            StartData = command[1].Split("=");
            return command[1].StartsWith(CommandText);
        }
    }
}