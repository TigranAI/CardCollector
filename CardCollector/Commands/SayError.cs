using System;
using System.Threading.Tasks;
using CardCollector.Attributes.Menu;
using CardCollector.Database;
using CardCollector.Database.Entity;
using CardCollector.Resources;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands
{
    [DontAddToCommandStack]
    public class SayError : HandlerModel
    {
        private Exception _exception;

        protected override string CommandText => "";
        protected override async Task Execute()
        {
            await User.Messages.SendMessage(User, $"{Messages.unexpected_exception} {_exception.Message}");
            Logs.LogOutError(_exception);
        }

        public override bool Match() => true;
        
        public SayError(User user, BotDatabaseContext context, Exception exception) : base(user, context)
        {
            _exception = exception;
        }
    }
}